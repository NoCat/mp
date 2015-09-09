using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aliyun.OpenServices.OpenStorageService;
using System.IO;
using System.Configuration;

static public class OssFile
{
    static OssClient _client = null;
    static string _accessId = ConfigurationManager.AppSettings["OssAccessId"];
    static string _accessKey = ConfigurationManager.AppSettings["OssAccessKey"];
    static string _bucketName = ConfigurationManager.AppSettings["OssPublicBucketName"];
    static string _endPoint = ConfigurationManager.AppSettings["OssEndPoint"];

    static OssFile()
    {
        _client = new OssClient(_endPoint, _accessId, _accessKey);
    }

    public static void ClearBucket()
    {
        var list = _client.ListObjects(_bucketName);
        while (true)
        {
            List<string> keyList = new List<string>();
            foreach (var item in list.ObjectSummaries)
            {
                keyList.Add(item.Key);
            }
            _client.DeleteObjects(new DeleteObjectsRequest(_bucketName, keyList));

            if (list.NextMarker != null)
            {
                list = _client.ListObjects(new ListObjectsRequest(_bucketName) { Marker = list.NextMarker });
            }
            else
            {
                break;
            }
        }
    }

    public static void Create(string path, Stream s)
    {
        int count = 0;
        while (count < 5)
        {
            try
            {
                s.Position = 0;
                _client.PutObject(_bucketName, path, s, new ObjectMetadata());
                break;
            }
            catch
            {
                count++;
            }
        }
    }

    public static void Delete(string path)
    {
        _client.DeleteObject(_bucketName, path);
    }

    public static void Delete(IList<string> list)
    {
        _client.DeleteObjects(new DeleteObjectsRequest(_bucketName, list));
    }

    public static void Move(string source, string destination)
    {
        var request = new CopyObjectRequest(_bucketName, source, _bucketName, destination);
        try
        {
            _client.CopyObject(request);
            Delete(source);
        }
        catch { }
    }

    public static bool IsFileExist(string path)
    {
        try
        {
            _client.GetObjectMetadata(_bucketName, path);
            return true;
        }
        catch { return false; }

    }

    public static Stream Open(string path)
    {
        Stream s = new MemoryStream();
        _client.GetObject(new GetObjectRequest(_bucketName, path), s);
        s.Position = 0;
        return s;
    }

    public static ObjectListing ListObjects()
    {
        return _client.ListObjects(_bucketName);
    }

    public static ObjectListing ListObjects(string maker)
    {
        ListObjectsRequest request = new ListObjectsRequest(_bucketName);
        request.Marker = maker;
        return _client.ListObjects(request);
    }
}