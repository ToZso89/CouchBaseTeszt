using Couchbase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CouchBaseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Cluster bucket = new Cluster())
            {
                var openedbucket = bucket.OpenBucket();
                var document = new Document<dynamic>
                {
                    Id = "HelloAgain2",
                    Content = new
                    {
                        name = "Couchbase2",
                        trully = "True",
                        goody = "Good",
                        location = "Budapest"
                    }
                };
                var valami = document.Content;
                var upsert = openedbucket.Upsert(document);
                if(upsert.Success)
                {
                    var get = openedbucket.GetDocument<dynamic>(document.Id);
                    document = get.Document;
                    var msg = string.Format("{0} {1}!", document.Id, document.Content.name);
                    Dictionary<string, object> toAddObject = new Dictionary<string, object>();
                    foreach(JProperty a in document.Content)
                    {
                        toAddObject.Add(a.Name, a.Value);
                    }
                    toAddObject.Add("type", "Gambling");
                    var toAddDocument = new Document<dynamic>
                    {
                        Id = document.Id + "2",
                        Content = toAddObject
                    };
                    var upsert2 = openedbucket.Upsert(toAddDocument);
                    if(upsert2.Success)
                    {
                        Console.WriteLine("Success");
                    }
                    else
                    {
                        Console.WriteLine("Fail");
                    }
                    Console.WriteLine(msg);
                }
                Console.ReadLine();
            }
        }
    }
}
