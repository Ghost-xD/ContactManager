using Microsoft.AspNetCore.Mvc;
using ContactManager.Models;
using ContactManager.Helper;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace ContactManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly IMemoryCache _cache;
        //private const string keyContactsResponse = "ContactsResponse";
        private const string keyContacts = "Contacts";
        private const string DataPath = "Data/Person.json";
        public ContactsController(ILogger<ContactsController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        [HttpGet("{pageNum}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetData(int pageNum = 0, int pageSize = 10)
        {
            try
            {
                if (_cache.TryGetValue(keyContacts, out var data))
                {
                    return Ok(data);
                }

                int totalRecords = 0;
                int totalPages = 0;

                UtilityClass util = new UtilityClass();
                List<Contacts> contacts = await util.LoadDataAsync(DataPath, "json");

                if (contacts.Count == 0)
                    return NotFound();

                totalRecords = contacts.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                //IEnumerable<Contacts> contactRes = contacts.Where(x => !x.IsDeleted).Skip(pageNum * pageSize).Take(pageSize).ToList();
                IEnumerable<Contacts> contactRes = contacts.Where(x => !x.IsDeleted).ToList();

                var response = new ContactResponse
                {
                    Contacts = contactRes
                    //TotalPages = totalPages,
                    //TotalRecords = totalRecords
                };

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                //_cache.Set(keyContactsResponse, response);
                _cache.Set(keyContacts, contactRes);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var error = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
                return error;
            }
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Contacts>> GetDataById(int Id)
        {
            try
            {
                List<Contacts> contacts = new();
                UtilityClass util = new UtilityClass();

                if (_cache.TryGetValue(keyContacts, out var data))
                    contacts = (List<Contacts>)data;
                else
                    contacts = await util.LoadDataAsync(DataPath, "json");

                if (contacts.Count == 0)
                    return NotFound();

                Contacts currentContact = contacts.Where(x => x.Id == Id && !x.IsDeleted).SingleOrDefault();

                return currentContact;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var error = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
                return error;
            }
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteData(int Id)
        {
            try
            {
                List<Contacts> dbContacts = new();
                UtilityClass util = new UtilityClass();

                if (_cache.TryGetValue(keyContacts, out var data))
                    dbContacts = (List<Contacts>)data;
                else
                    dbContacts = await util.LoadDataAsync(DataPath, "json");

                if (dbContacts.Count > 0)
                {
                    Contacts contactToDelete = dbContacts.Where(x => x.Id == Id).SingleOrDefault();
                    if (contactToDelete == null)
                        return NotFound();

                    contactToDelete.IsDeleted = true;
                    string updatedJson = JsonConvert.SerializeObject(dbContacts, Formatting.Indented);
                    await util.SaveJsonAsync(DataPath, updatedJson);
                }

                List<Contacts> dataToBeCached = dbContacts.Where(x => !x.IsDeleted).ToList();

                _cache.Set(keyContacts, dataToBeCached);

                return Ok(dbContacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var error = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
                return error;
            }
        }
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateData([FromBody] Contacts contact)
        {
            try
            {
                List<Contacts> dbContacts = new();
                UtilityClass util = new UtilityClass();

                if (_cache.TryGetValue(keyContacts, out var data))
                    dbContacts = (List<Contacts>)data;
                else
                    dbContacts = await util.LoadDataAsync(DataPath, "json");

                if (dbContacts.Count > 0)
                {
                    Contacts contactToUpdate = dbContacts.Where(x => x.Id == contact.Id).SingleOrDefault();
                    contactToUpdate.FirstName = contact.FirstName;
                    contactToUpdate.LastName = contact.LastName;
                    contactToUpdate.Email = contact.Email;

                    string updatedJson = JsonConvert.SerializeObject(dbContacts, Formatting.Indented);
                    await util.SaveJsonAsync(DataPath, updatedJson);
                }

                List<Contacts> dataToBeCached = dbContacts.Where(x => !x.IsDeleted).ToList();

                _cache.Set(keyContacts, dataToBeCached);

                return Ok(dbContacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var error = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
                return error;
            }
        }
        [HttpPost("Create")]
        public async Task<ActionResult<IEnumerable<Contacts>>> CreateData(Contacts contact)
        {
            try
            {
                List<Contacts> dbContacts = new();
                UtilityClass util = new UtilityClass();

                if (_cache.TryGetValue(keyContacts, out var data))
                    dbContacts = (List<Contacts>)data;
                else
                    dbContacts = await util.LoadDataAsync(DataPath, "json");

                int currMaxId = dbContacts.Max(x => x.Id);

                Contacts newcontact = new Contacts
                {
                    Id = currMaxId + 1,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    IsDeleted = false
                };

                //dbContacts.Add(newcontact);
                dbContacts.Insert(0, newcontact);

                string updatedJson = JsonConvert.SerializeObject(dbContacts, Formatting.Indented);
                await util.SaveJsonAsync(DataPath, updatedJson);

                _cache.Set(keyContacts, dbContacts);

                return CreatedAtAction(nameof(GetDataById), new { Id = newcontact.Id }, newcontact);                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var error = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
                return error;
            }
        }
    }
}