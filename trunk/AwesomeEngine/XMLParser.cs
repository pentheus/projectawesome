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

            //Load world geometry
            readGeometry(scenedoc);
            //Load objects
            readObjects(scenedoc);
        }

        public ModelInfo readGeometry(XmlDocument scenedoc)
        {
            ModelInfo geometry = new ModelInfo();
            return geometry;
        }

        public List<ModelInfo> readObjects(XmlDocument scenedoc)
        {
            //Generate a list of the objects to be placed in the scene
            XmlNodeList models = scenedoc.GetElementsByTagName("Object");
            List<ModelInfo> objects = new List<ModelInfo>();
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
                    ModelInfo obj = new ModelInfo(objvect, objrot, objscale, null, null, null);
                    objects.Add(obj);
                }
                catch (FormatException)
                {
                    Console.WriteLine("XML not properly formatted: Value in object with model "
                        + node.SelectSingleNode("model").InnerText);
                }
            }
            return objects;
        }
        public Boolean saveScene(Octree scene)
        {
            ModelInfo savegeo = scene.getGeometry();
            List<ModelInfo> objects = scene.getDrawableObjects();
            XmlTextWriter scenesaver = new XmlTextWriter("C:/Documents and Settings/Alex/My Documents/Inf 125/ProjectAwesome/Scenedata.xml", null);
            scenesaver.WriteStartDocument();
            //Write the world geometry to the file
            saveGeometry(scenesaver, savegeo);
            //Write objects to the file
            saveObjects(scenesaver, objects);
            scenesaver.WriteEndDocument();
            scenesaver.Flush();
            scenesaver.Close();
            return true;
        }

        public void saveGeometry(XmlTextWriter scenesaver, ModelInfo geometry)
        {
            scenesaver.WriteStartElement("Scene");
            scenesaver.WriteStartElement("World Geometry");
            scenesaver.WriteStartElement("model");
            scenesaver.WriteString("Blah");
            scenesaver.WriteEndElement();
            scenesaver.WriteEndElement();
        }

        public void saveObjects(XmlTextWriter scenesaver, List<ModelInfo> objects)
        {
            foreach(ModelInfo obj in objects)
            {
                scenesaver.WriteStartElement("Conent");
                scenesaver.WriteStartElement("Object");

                scenesaver.WriteStartElement("posx");
                scenesaver.WriteString(obj.Position.X.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("posy");
                scenesaver.WriteString(obj.Position.Y.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("posz");
                scenesaver.WriteString(obj.Position.Z.ToString());
                scenesaver.WriteEndElement();

                scenesaver.WriteStartElement("rotx");
                scenesaver.WriteString(obj.Rotation.X.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("roty");
                scenesaver.WriteString(obj.Rotation.Y.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("rotz");
                scenesaver.WriteString(obj.Rotation.Z.ToString());
                scenesaver.WriteEndElement();

                scenesaver.WriteStartElement("scalex");
                scenesaver.WriteString(obj.Scale.X.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("scaley");
                scenesaver.WriteString(obj.Scale.Y.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("scalez");
                scenesaver.WriteString(obj.Scale.Z.ToString());
                scenesaver.WriteEndElement();

                scenesaver.WriteStartElement("model");
                scenesaver.WriteString(obj.Model.Tag.ToString());
                scenesaver.WriteEndElement();

                scenesaver.WriteEndElement();
                scenesaver.WriteEndElement();
            }
        }
    }
}