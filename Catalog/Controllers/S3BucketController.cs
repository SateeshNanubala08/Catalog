using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;  
using Amazon;  
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Catalog.Models;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3BucketController : ControllerBase
    {
        IAmazonS3 S3Client { get; set; }
        public string filePath = "New.json";

        public readonly IAmazonS3 amazonS3; 
        private readonly ServiceConfiguration _settings;

        private readonly IAmazonS3 _S3Bucket;
        //private readonly IAWSS3BucketHelper _AWSS3BucketHelper;


        public S3BucketController(IAmazonS3 s3Client, IOptions<ServiceConfiguration> settings )
        {
            this.S3Client = s3Client;

            this._settings = settings.Value;
            this._S3Bucket = new AmazonS3Client(this._settings.AWSS3.AccessKey, this._settings.AWSS3.SecretKey, RegionEndpoint.USEast1);
           
            // this._S3Bucket = new AmazonS3Client("AKIAT22HSWCUK7NKI4NR", "hClY7w8JdtTtWqK5NsTRNoiKv5rmoPR3Q+IriTI7", RegionEndpoint.USEast1);


        }

        [HttpPost("CreateFolder")]
        public async Task<int> CreateFolder(string bucketname, string newFolderName, string prefix = "")
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = bucketname;
            request.Key = (prefix.TrimEnd('/') + "/" + newFolderName.TrimEnd('/') + "/").TrimStart('/');
            var response = await S3Client.PutObjectAsync(request);
            return (int)response.HttpStatusCode;

        }


      

        [HttpPost("UploadFile")]
        public async Task<int> UploadFile(string bucketName, string keyName)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);

            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = _settings.AWSS3.BucketName, // "sateeshmphasisbucket1", //

                    Key = filePath,
                    FilePath = filePath,
                    ContentType = "text/plain"
                };

                var response = await S3Client.PutObjectAsync(putRequest);
                return (int)response.HttpStatusCode;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return 400;
                }
                else
                {
                    return 400;
                }

            }
        }

        [HttpGet("ReadS3Obj")]
        public async Task<string> ReadS3Obj(string S3_KEY)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = _settings.AWSS3.BucketName; // "sateeshmphasisbucket1"; //
            request.Key = S3_KEY;

            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            //List<masterjson> dese = JsonConvert.DeserializeObject< List<masterjson>>(content);
            //masterjson result = dese.Single(a => a.id==1);

             
            return content;
        }


        [HttpGet("ReadObjID")]
        public async Task<Catlogjson> ReadObjbyID(int id)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = _settings.AWSS3.BucketName; // "sateeshmphasisbucket1";
            request.Key = filePath;

            GetObjectResponse response = await client.GetObjectAsync(request);

            StreamReader reader = new StreamReader(response.ResponseStream);

            string content = reader.ReadToEnd();
            List<Catlogjson> dese = JsonConvert.DeserializeObject<List<Catlogjson>>(content);
            Catlogjson result = dese.Single(a => a.CatId == id);


            //Console.Out.WriteLine("Read S3 object with key " + S3_KEY + " in bucket " + BUCKET_NAME + ". Content is: " + content);
            return result;
        }

        [HttpPost("CreateData")]
        public async Task<string> CreateData(int Catid, string CatName, double Price, string Remarks)
        {
            filePath = Catid + ".Json";
            Catlogjson inst = new Catlogjson();
            inst.CatId = Catid;
            inst.CatName = CatName;
            inst.CDate = DateTime.Now;
            inst.Price = Price;
            inst.Remarks = Remarks;

            //var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            //GetObjectRequest request = new GetObjectRequest();
            //request.BucketName = "sateeshmphasisbucket1";
            //request.Key = filePath;
            //GetObjectResponse response = await client.GetObjectAsync(request);
            //StreamReader reader = new StreamReader(response.ResponseStream);
            //string content = reader.ReadToEnd();
            //List<Catlogjson> dese = JsonConvert.DeserializeObject<List<Catlogjson>>(content);
            //dese.Add(inst); 


            string serializedval = JsonConvert.SerializeObject(inst);

            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllText(filePath, serializedval);

            PutObjectRequest putRequest = new PutObjectRequest
            {
                BucketName = _settings.AWSS3.BucketName, // "sateeshmphasisbucket1",

            Key = filePath,
                FilePath = filePath ,
                ContentType = "application/json"
            };

            var response2 = await S3Client.PutObjectAsync(putRequest); 

            return response2.ToString();
        }

        [HttpPut("UpdateData")]
        public async Task<string> UpdateData(int Catid , string CatName, double Price , string Remarks )
        {
            Catlogjson inst = new Catlogjson();  
            inst.CatId = Catid;
            inst.CatName = CatName;
            inst.CDate = DateTime.Now;
            inst.Price = Price;
            inst.Remarks = Remarks;

            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = _settings.AWSS3.BucketName; // "sateeshmphasisbucket1";
            request.Key = "Catlogjson";


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

        //[HttpDelete("DeleteData")]
        //public async Task<string> DeleteData(int id)
        //{
        //    Catlogjson inst = new Catlogjson();

        //    inst.CatId = id;
        //    var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
        //    GetObjectRequest request = new GetObjectRequest();

        //    request.BucketName = "sateeshmphasisbucket1";
        //    request.Key = "masterjson";


        //    GetObjectResponse response = await client.GetObjectAsync(request);

        //    StreamReader reader = new StreamReader(response.ResponseStream);

        //    string content = reader.ReadToEnd();
        //    List<Catlogjson> dese = JsonConvert.DeserializeObject<List<Catlogjson>>(content);
        //    dese.RemoveAll(x => x.CatId == id);
        //    //dese.Add(inst);
        //    //string serializedval =JsonSerializer. .ser(dese);
        //    string serializedval = JsonConvert.SerializeObject(dese);

        //    System.IO.File.WriteAllText(filePath, string.Empty);
        //    System.IO.File.WriteAllText(filePath, serializedval);

        //    PutObjectRequest putRequest = new PutObjectRequest
        //    {
        //        BucketName = "sateeshmphasisbucket1",

        //        Key = "Catlogjson",
        //        FilePath = filePath,
        //        ContentType = "application/json"
        //    };

        //    var response2 = S3Client.PutObjectAsync(putRequest); 

        //    return content;
        //}


        [HttpDelete("DeleteData")]
        public async Task<string> DeleteData(string id)
        {
            DeleteObjectResponse response = await S3Client.DeleteObjectAsync(_settings.AWSS3.BucketName, id);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                return response.ToString();
            else
                return "Error";
        }


        public class Catlogjson
        {

            //[Key]
            //public int CatId { get; set; }

            //public String CatName { get; set; }

            //public Double Price { get; set; }

            //public DateTime CDate { get; set; }
            //public string Remarks { get; set; }



            [JsonPropertyName("Cagtid")]

            public int CatId { get; set; }

            [JsonPropertyName("CatName")]

            public string CatName { get; set; }

            [JsonPropertyName("CDate")]

            public DateTime CDate { get; set; }

            [JsonPropertyName("Price")]

            public double Price { get; set; }

            [JsonPropertyName("Remarks")]

            public string Remarks { get; set; }


        }

    }
}

