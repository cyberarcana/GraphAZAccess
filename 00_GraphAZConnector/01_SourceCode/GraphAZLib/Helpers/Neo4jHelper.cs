using GraphAZLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAZLib.Helpers
{
    public static class Neo4jHelper
    {
        public static List<NodeEntity> GetNodes(string aFilePath)
        {
            List<NodeEntity> lNodes = new List<NodeEntity>();

            using (StreamReader sr = new StreamReader(aFilePath))
            {
                while (sr.Peek() >= 0)
                {
                    var jsonLine = sr.ReadLine();

                    if (jsonLine != null)
                    {
                        var lNewNode = JsonConvert.DeserializeObject<NodeEntity>(jsonLine);

                        if (lNewNode != null && lNewNode.Type.ToLower() == "node")
                        {
                            lNodes.Add(lNewNode);
                        }
                    }                    
                }
            }            

            return lNodes;
        }

        public static List<RelationshipEntity> GetRelationships(string aFilePath)
        {
            List<RelationshipEntity> lRelationships = new List<RelationshipEntity>();

            using (StreamReader sr = new StreamReader(aFilePath))
            {
                while (sr.Peek() >= 0)
                {
                    var jsonLine = sr.ReadLine();
                    var lNewRel = JsonConvert.DeserializeObject<RelationshipEntity>(jsonLine);

                    if (lNewRel != null && lNewRel.Type.ToLower() == "relationship")
                    {
                        lRelationships.Add(lNewRel);
                    }
                }
            }

            return lRelationships;
        }
    }
}
