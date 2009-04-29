using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AwesomeEngine;

namespace TestScene
{
    class XMLParser
    {
        public void readScene(string filename)
        {
            XmlTextReader scenereader = new XmlTextReader(filename);
            XmlDocument scenedoc = new XmlDocument();
            scenedoc.Load(scenereader);

            //Generate a list of the objects to be placed in the scene
            XmlNodeList models = scenedoc.GetElementsByTagName("Object");
            Console.WriteLine(models.Count);
            //Create a ModelInfo object for each object, then add it to the octree
            foreach (XmlNode node in models)
            {
                try
                {
                    int objx = Convert.ToInt32(node.SelectSingleNode("posx").InnerText);
                    int objy = Convert.ToInt32(node.SelectSingleNode("posy").InnerText);
                    int objz = Convert.ToInt32(node.SelectSingleNode("posz").InnerText);
                    Vector3 objvect = new Vector3(objx, objy, objz);

                    int objrotx = Convert.ToInt32(node.SelectSingleNode("rotx").InnerText);
                    int objroty = Convert.ToInt32(node.SelectSingleNode("roty").InnerText);
                    int objrotz = Convert.ToInt32(node.SelectSingleNode("rotz").InnerText);
                    Vector3 objrot = new Vector3(objrotx, objroty, objrotz);

                    int objscalex = Convert.ToInt32(node.SelectSingleNode("scalex").InnerText);
                    int objscaley = Convert.ToInt32(node.SelectSingleNode("scaley").InnerText);
                    int objscalez = Convert.ToInt32(node.SelectSingleNode("scalez").InnerText);
                    Vector3 objscale = new Vector3(objscalex, objscaley, objscalez);

                    String modelname = node.SelectSingleNode("model").InnerText;

                    //BoundingSphere objbound = new BoundingSphere(objvect, objmodel.g

                    //Create the ModelInfo object
                    //ModelInfo obj = new ModelInfo(objvect, objrot, objscale, null, null, null);
                }
                catch (FormatException)
                {
                    Console.WriteLine("XML not properly formatted: Value in object with model "
                        + node.SelectSingleNode("model").InnerText);
                }
            }
        }

    }
}