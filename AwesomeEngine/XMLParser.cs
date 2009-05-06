using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AwesomeEngine;
using System.Collections;

namespace TestScene
{
    class XMLParser
    {
        Game game;

        public XMLParser(Game received)
        {
            game = received;
        }

        public Octree readScene(string filename)
        {
            XmlTextReader scenereader = new XmlTextReader(filename);
            XmlDocument scenedoc = new XmlDocument();
            Octree scene = new Octree(1);
            scenedoc.Load(scenereader);

            //Load world geometry
            readGeometry(scenedoc, scene);
            //Load objects
            readObjects(scenedoc, scene);

            return scene;
        }

        public void readGeometry(XmlDocument scenedoc, Octree scene)
        {
            XmlNode geometrynode = scenedoc.GetElementsByTagName("WorldGeometry").Item(0);

            String modelname = geometrynode.SelectSingleNode("model").InnerText;
            Model objmodel = game.Content.Load<Model>(modelname);

            int geoscalex = Convert.ToInt32(geometrynode.SelectSingleNode("scalex").InnerText);
            int geoscaley = Convert.ToInt32(geometrynode.SelectSingleNode("scaley").InnerText);
            int geoscalez = Convert.ToInt32(geometrynode.SelectSingleNode("scalez").InnerText);
            Vector3 geoscale = new Vector3(geoscalex, geoscaley, geoscalez);

            ModelInfo geometry = new ModelInfo(new Vector3(0), new Vector3(0), geoscale,
                objmodel, null);
            scene.addGeometry(geometry);
        }

        public void readObjects(XmlDocument scenedoc, Octree scene)
        {
            //Generate a list of the objects to be placed in the scene
            XmlNodeList models = scenedoc.GetElementsByTagName("Object");
            List<ModelInfo> objects = new List<ModelInfo>();
            Hashtable modelsloaded = new Hashtable();
            //Create a ModelInfo object for each object, then add it to the octree
            foreach (XmlNode node in models)
            {
                try
                {
                    float objx = (float)Convert.ToDouble(node.SelectSingleNode("posx").InnerText);
                    float objy = (float)Convert.ToDouble(node.SelectSingleNode("posy").InnerText);
                    float objz = (float)Convert.ToDouble(node.SelectSingleNode("posz").InnerText);
                    Vector3 objvect = new Vector3(objx, objy, objz);

                    float objrotx = (float)Convert.ToDouble(node.SelectSingleNode("rotx").InnerText);
                    float objroty = (float)Convert.ToDouble(node.SelectSingleNode("roty").InnerText);
                    float objrotz = (float)Convert.ToDouble(node.SelectSingleNode("rotz").InnerText);
                    Vector3 objrot = new Vector3(objrotx, objroty, objrotz);

                    float objscalex = (float)Convert.ToDouble(node.SelectSingleNode("scalex").InnerText);
                    float objscaley = (float)Convert.ToDouble(node.SelectSingleNode("scaley").InnerText);
                    float objscalez = (float)Convert.ToDouble(node.SelectSingleNode("scalez").InnerText);
                    Vector3 objscale = new Vector3(objscalex, objscaley, objscalez);

                    String modelname = node.SelectSingleNode("model").InnerText;
                    Model objmodel;
                    if (modelsloaded.Contains(modelname))
                        objmodel = (Model)modelsloaded[modelname];
                    else
                    {
                        objmodel = game.Content.Load<Model>(modelname);
                        modelsloaded.Add(modelname, objmodel);
                    }

                    //Create the ModelInfo object
                    ModelInfo obj = new ModelInfo(objvect, objrot, objscale, objmodel, null);
                    scene.addObject(objvect, obj);
                }
                catch (FormatException)
                {
                    Console.WriteLine("XML not properly formatted: Value in object with model "
                        + node.SelectSingleNode("model").InnerText);
                }
            }
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

            scenesaver.WriteStartElement("scalex");
            scenesaver.WriteString(geometry.Scale.X.ToString());
            scenesaver.WriteEndElement();
            scenesaver.WriteStartElement("scaley");
            scenesaver.WriteString(geometry.Scale.Y.ToString());
            scenesaver.WriteEndElement();
            scenesaver.WriteStartElement("scalez");
            scenesaver.WriteString(geometry.Scale.Z.ToString());
            scenesaver.WriteEndElement();

            scenesaver.WriteStartElement("model");
            scenesaver.WriteString("blah");
            scenesaver.WriteEndElement();
            scenesaver.WriteEndElement();
        }

        public void saveObjects(XmlTextWriter scenesaver, List<ModelInfo> objects)
        {
            foreach (ModelInfo obj in objects)
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