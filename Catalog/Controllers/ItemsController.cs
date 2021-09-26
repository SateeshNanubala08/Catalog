using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Catalog.Entities;
using Catalog.Helpers;
using Catalog.Models;
using Catalog.Repositories; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Catalog.Controllers.S3BucketController;
using static Catalog.Dtos.Dtos;

namespace Catalog.Controllers
{

    [ApiController]
    [Route("items")]

    public class ItemsController : ControllerBase
    {
         private readonly ILogger<ItemsController> _logger;
       
        //private IFileRepository f_awsRepository;

        public readonly IAmazonS3 amazonS3;
        private readonly ServiceConfiguration _settings;        
        private readonly IAmazonS3 _S3Bucket;
        private readonly IAWSS3BucketHelper _AWSS3BucketHelper;
        IAmazonS3 S3Client { get; set; }
        public string Message { get; set; }

        public ItemsController(IAmazonS3 amazonS3, IOptions<ServiceConfiguration> settings , IAmazonS3 s3Client, ILogger<ItemsController> logger )
        { 
            _logger = logger;
            this.amazonS3 = amazonS3;
            this.S3Client = s3Client;
           // this.f_awsRepository = awsRepository;

            //this._AWSS3BucketHelper = AWSS3BucketHelper;

            this._settings = settings.Value;
            this._S3Bucket = new AmazonS3Client(this._settings.AWSS3.AccessKey, this._settings.AWSS3.SecretKey, RegionEndpoint.USEast1);
            //this._S3Bucket = new AmazonS3Client("AKIAT22HSWCUK7NKI4NR", "hClY7w8JdtTtWqK5NsTRNoiKv5rmoPR3Q+IriTI7", RegionEndpoint.USEast1);

        }

        public ItemsController()
        {
        }

        //public void OnGet()
        //{
        //    _logger.LogInformation("GET Pages.PrivacyModel called.");
        //}

        //public static readonly List<ItemDto> items = new()
        //{
        //    new ItemDto(1, "Sateesh", DateTime.UtcNow, 100, ""),
        //    new ItemDto(2, "Prasad", DateTime.UtcNow, 200, ""),
        //    new ItemDto(3, "Raju", DateTime.UtcNow, 300, ""),
        //    new ItemDto(4, "Mahe", DateTime.UtcNow, 400, "")

        //};

        private readonly ItemRepository itemRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {

            //List<string> files = await f_awsRepository.FilesList();
            //List<Models.Catalog> lstCatalog = new List<Models.Catalog>();
            //foreach (var fileName in files)
            //{
            //    Stream stream = await f_awsRepository.GetFile(fileName);

            //    StreamReader reader = new StreamReader(stream);
            //    string text = reader.ReadToEnd();

            //    Models.Catalog catalog = System.Text.Json.JsonSerializer.Deserialize<Models.Catalog>(text);
            //    lstCatalog.Add(catalog);
            //}

            ////return lstCatalog;

            //var items = (await itemRepository.GetItemsAsync())
            //    .Select(files=> files.AsDto());

            var items = (await itemRepository.GetItemsAsync())
                .Select(itemRepository => itemRepository.AsDto()); 

            return items;


            //========================================

            //AmazonS3Client client = new AmazonS3Client();
            //ListObjectsRequest listRequest = new ListObjectsRequest
            //{
            //    BucketName = "SampleBucket",
            //};

            //ListObjectsResponse listResponse;
            //do
            //{
            //    // Get a list of objects
            //    //listResponse = client.ListObjects(listRequest);
            //     listResponse =  client.ListObjects(listRequest);
            //    foreach (S3Object obj in listResponse.S3Objects)
            //    {
            //        //Console.WriteLine("Object - " + obj.Key);
            //        //Console.WriteLine(" Size - " + obj.Size);
            //        //Console.WriteLine(" LastModified - " + obj.LastModified);
            //        //Console.WriteLine(" Storage class - " + obj.StorageClass);


            //    }

            //    // Set the marker property
            //    listRequest.Marker = listResponse.NextMarker;
            //} while (listResponse.IsTruncated);


        }



        //[HttpGet("{id}")]
        //public async Task<ActionResult<ItemDto>> GetByIdAsync(int id)
        //{
        //    var item = await itemRepository.GetAsync(id);


        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    return item.AsDto();
        //}



        [HttpGet("GetAll")]
        public async Task<string> GetAll()
        {
            string content = "";
            try
            {
                var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                GetObjectRequest request = new GetObjectRequest();

                request.BucketName = _settings.AWSS3.BucketName;//  "sateeshmphasisbucket1";
                request.Key = "1.Json";

                GetObjectResponse response = await client.GetObjectAsync(request);

                StreamReader reader = new StreamReader(response.ResponseStream);

                content = reader.ReadToEnd();
                return content;



                //=====================================
                //List<string> files = await f_awsRepository.FilesList();
                //List<Models.Catalog> lstCatalog = new List<Models.Catalog>();
                //foreach (var fileName in files)
                //{
                //    Stream stream = await f_awsRepository.GetFile(fileName);

                //    StreamReader reader = new StreamReader(stream);
                //    string text = reader.ReadToEnd();

                //    Models.Catalog catalog = System.Text.Json.JsonSerializer.Deserialize<Models.Catalog>(text);
                //    lstCatalog.Add(catalog);

                //}

                //content = lstCatalog.ToString();  
                // return content;


            }
            catch (Exception ex)
            {
                content = ex.Message; 

                _logger.Log(LogLevel.Information, content);
                _logger.LogInformation(content);
               

                return content;
            }
           
        }


        [HttpGet("GetbyID")]
        public async Task<Catalog.Models.Catalog> GetbyID(int id)
        {
            Catalog.Models.Catalog result;

            try
            {
                var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                GetObjectRequest request = new GetObjectRequest();

                request.BucketName = _settings.AWSS3.BucketName; // "sateeshmphasisbucket1";
                request.Key = "New.Json";

                GetObjectResponse response = await client.GetObjectAsync(request);

                StreamReader reader = new StreamReader(response.ResponseStream);

                string content = reader.ReadToEnd();
                List<Catalog.Models.Catalog> dese = JsonConvert.DeserializeObject<List<Catalog.Models.Catalog>>(content);
                                result = dese.Single(a => a.CatId == id);


                return result;

            } 
            catch (Exception ex)
            {
                 _logger.Log(LogLevel.Information, ex.Message);
                 _logger.LogInformation(ex.Message);

                return null;
            }


            
        }


        [HttpPost]
        public  ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {

            //var item = new Items
            //{
            //    CatId = createItemDto.CatId,
            //    CatName = createItemDto.CatName,
            //    CDate = DateTime.Now,
            //    Price = createItemDto.Price,
            //    Remarks = createItemDto.Remarks
            //};
            //await itemRepository.CreateAsync(item); 
            //items.Add(item);
            //return CreatedAtAction(nameof(GetByIdAsync), new { id = item.CatId }, item);



            //var item = new ItemDto(createItemDto.CatId, createItemDto.CatName, DateTime.Now, createItemDto.Price, createItemDto.Remarks);
            //string ContentData = JsonConvert.SerializeObject(item);



            ////System.IO.File.WriteAllText(filePath, string.Empty);
            //System.IO.File.WriteAllText(filePath, ContentData);


            //var putRequest = new PutObjectRequest()
            //{
            //    BucketName = "sateeshmphasisbucket1",// _settings.AWSS3.BucketName,
            //    Key = filePath,
            //    ContentType = "text/plain"
            //};

            //var result = this.amazonS3.PutObjectAsync(putRequest);

            //return CreatedAtAction(nameof(GetByIdAsync), new { id = item.CatId }, item);


            string filePath = createItemDto.CatId + ".Json";

            Catalog.Models.Catalog inst = new Catalog.Models.Catalog();
            inst.CatId = createItemDto.CatId;
            inst.CatName = createItemDto.CatName;
            inst.CDate = DateTime.Now;
            inst.Price = createItemDto.Price;
            inst.Remarks = createItemDto.Remarks;

            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1); 
            string serializedval = JsonConvert.SerializeObject(inst);

            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllText(filePath, serializedval);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName =  _settings.AWSS3.BucketName,  

                Key = filePath,
                FilePath = filePath,
                ContentType = "application/json"
                
            };

            var response2 = this.amazonS3.PutObjectAsync(putRequest);

            //return CreatedAtAction(nameof(GetByIdAsync), new { id = inst.CatId }, inst); 
            return CreatedAtAction(nameof(GetbyID), new { id = inst.CatId }, inst);  

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id , UpdateItemDto updateItemDto)
        {
            //var existingItem = items.Where(item => item.CatId == id).SingleOrDefault();


            //if (existingItem == null)
            //{
            //    return NotFound();

            //}
            //var updateditem = existingItem with
            //{
            //    CatName = updateItemDto.CatName,
            //    Price = updateItemDto.Price

            //};


            //var index = items.FindIndex(existingItem => existingItem.CatId == id);
            //items[index] = updateditem;


            var existingItem = await itemRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.CatName = updateItemDto.CatName;
            existingItem.Price = updateItemDto.Price;
            existingItem.Remarks = updateItemDto.Remarks;

            await itemRepository.UpdateAsync(existingItem);


            //string filePath = id + ".Json";
            //var path = Path.Combine("Files", filePath);
            //using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
            //{
            //    string fileExtension = Path.GetExtension(path);
            //    string fileName = string.Empty;
            //    fileName = filePath;
            //    var response =  _AWSS3BucketHelper.UploadFile(fsSource, fileName);
            //}


            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {

            string filePath = id + ".Json";

            //var item = await itemRepository.GetAsync(id);

            //if (item == null)
            //{
            //    return NotFound(); 
            //}

            // items.RemoveAt(index) ;

            //await itemRepository.RemoveAsync(item.CatId);

            var response = await _AWSS3BucketHelper.DeleteFile(filePath);

            return NoContent();
        }



        [HttpPut("UpdateData")]
        public async Task<string> UpdateData(int Catid, string CatName, double Price, string Remarks)
        {

            string filePath = Catid + ".Json";

            Catlogjson inst = new Catlogjson();
            inst.CatId = Catid;
            inst.CatName = CatName;
            inst.CDate = DateTime.Now;
            inst.Price = Price;
            inst.Remarks = Remarks;

            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = _settings.AWSS3.BucketName; // "sateeshmphasisbucket1";
            request.Key = filePath ;


            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            List<Catlogjson> dese = JsonConvert.DeserializeObject<List<Catlogjson>>(content);            
            dese.RemoveAll(x => x.CatId == Catid);
            dese.Add(inst);
            //string serializedval =JsonSerializer. .ser(dese);
            string serializedval = JsonConvert.SerializeObject(dese);

            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllText(filePath, serializedval);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = _settings.AWSS3.BucketName, // "sateeshmphasisbucket1",

                Key = filePath,
                FilePath = filePath,
                ContentType = "application/json"
            };

            var response2 = S3Client.PutObjectAsync(putRequest);

            return content;
        }



    }
}
