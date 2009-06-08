using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AwesomeEngine;
using AwesomeEngine.Items;
using System.Collections;

namespace AwesomeEngine
{
    public class XMLParser
    {
        ContainsScene game;

        public XMLParser(ContainsScene received)
        {
            game = received;
        }

        public Octree ReadScene(string filename)
        {
            XmlTextReader scenereader = new XmlTextReader(filename);
            XmlDocument scenedoc = new XmlDocument();
            scenedoc.Load(scenereader);

            XmlNode sceneinfo = scenedoc.GetElementsByTagName("SceneInfo").Item(0);
            float scenesize = (float)Convert.ToDouble(sceneinfo.SelectSingleNode("size").InnerText);
            
            Octree scene = new Octree(scenesize);
            //Load world geometry
            ReadGeometry(scenedoc, scene);
            //Load objects
            ReadObjects(scenedoc, scene);
            //Load items
            ReadItems(scenedoc, scene);
            //Load entities
            ReadEntities(scenedoc, scene);

            return scene;
        }

        private void ReadGeometry(XmlDocument scenedoc, Octree scene)
        {
            XmlNode geometrynode = scenedoc.GetElementsByTagName("WorldGeometry").Item(0);

            String modelname = geometrynode.SelectSingleNode("model").InnerText;
            Model objmodel = new Model();
            ModelInfo.LoadModel(ref objmodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), modelname, game.GetScene().Effect);

            float geoscalex = (float)Convert.ToDouble(geometrynode.SelectSingleNode("scalex").InnerText);
            float geoscaley = (float)Convert.ToDouble(geometrynode.SelectSingleNode("scaley").InnerText);
            float geoscalez = (float)Convert.ToDouble(geometrynode.SelectSingleNode("scalez").InnerText);
            Vector3 geoscale = new Vector3(geoscalex, geoscaley, geoscalez);

            ModelInfo geometry = new ModelInfo(new Vector3(0), new Vector3(0), geoscale,
                objmodel, modelname);
            scene.addGeometry(geometry);
        }

        private void ReadObjects(XmlDocument scenedoc, Octree scene)
        {
            //Generate a list of the objects to be placed in the scene
            XmlNodeList models = scenedoc.GetElementsByTagName("Object");
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
                    {
                        objmodel = (Model)modelsloaded[modelname];
                    }
                    else
                    {
                        objmodel = new Model();
                        ModelInfo.LoadModel(ref objmodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), modelname, game.GetScene().Effect);
                        modelsloaded.Add(modelname, objmodel);
                    }

                    //Create the ModelInfo object
                    ModelInfo obj = new ModelInfo(objvect, objrot, objscale, objmodel, modelname);
                    obj.BoundingSphere = ReadSphere(node.SelectSingleNode("BoundingSphere"));
                    scene.AddObject(obj);
                }
                catch (FormatException)
                {
                    Console.WriteLine("XML not properly formatted: Value in object with model "
                        + node.SelectSingleNode("model").InnerText);
                }
            }
        }

        public void ReadItems(XmlDocument scenedoc, Octree scene)
        {
            //Generate a list of the objects to be placed in the scene
            XmlNodeList items = scenedoc.GetElementsByTagName("Item");
            Hashtable modelsloaded = new Hashtable();
            //Create a ModelInfo object for each object, then add it to the octree
            foreach (XmlNode node in items)
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
                    {
                        objmodel = (Model)modelsloaded[modelname];
                    }
                    else
                    {
                        objmodel = new Model();
                        ModelInfo.LoadModel(ref objmodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), modelname, game.GetScene().Effect);
                        modelsloaded.Add(modelname, objmodel);
                    }

                    //Create the ModelInfo object
                    ModelInfo obj = new ModelInfo(objvect, objrot, objscale, objmodel, modelname);
                    
                    //Create the Item object
                    Item item;
                    
                    //Determine the type of item from what was listed
                    String itemtype = Convert.ToString(node.SelectSingleNode("itemtype").InnerText);
                    switch (itemtype)
                    {
                        case "BatteryItem":
                            item = new BatteryItem((Game)game, obj);
                            break;
                        case "FuseItem":
                            item = new FuseItem((Game)game, obj);
                            break;
                        case "GlowStickItem":
                            item = new GlowStickItem((Game)game, obj);
                            break;
                        default:
                            item = null;
                            break;
                    }
                    if (item != null)
                    {

                        item.BoundingSphere = ReadSphere(node.SelectSingleNode("BoundingSphere"));
                        scene.AddItem(item);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("XML not properly formatted: Value in object with model "
                        + node.SelectSingleNode("model").InnerText);
                }
            }
        }

        public void ReadEntities(XmlDocument scenedoc, Octree scene)
        {
            //Generate a list of the objects to be placed in the scene
            XmlNodeList entities = scenedoc.GetElementsByTagName("Entity");
            Hashtable modelsloaded = new Hashtable();
            //Create a ModelInfo object for each object, then add it to the octree
            foreach (XmlNode node in entities)
            {
                try
                {
                    float objx = (float)Convert.ToDouble(node.SelectSingleNode("posx").InnerText);
                    float objy = (float)Convert.ToDouble(node.SelectSingleNode("posy").InnerText);
                    float objz = (float)Convert.ToDouble(node.SelectSingleNode("posz").InnerText);
                    Vector3 objvect = new Vector3(objx, objy, objz);

                    String modelname = node.SelectSingleNode("model").InnerText;
                    Model objmodel;
                    if (modelsloaded.Contains(modelname))
                    {
                        objmodel = (Model)modelsloaded[modelname];
                    }
                    else
                    {
                        objmodel = new Model();
                        ModelInfo.LoadModel(ref objmodel, game.GetScene().Textures, game.GetContent(), game.GetGraphics(), modelname, game.GetScene().Effect);
                        modelsloaded.Add(modelname, objmodel);
                    }

                    //Create the Item object
                    LogicEntity entity;

                    //Determine the type of item from what was listed
                    String itemtype = Convert.ToString(node.SelectSingleNode("itemtype").InnerText);
                    switch (itemtype)
                    {
                        case "SpawnEntity":
                            entity = new SpawnEntity((Game)game, objmodel, objvect);
                            break;
                        case "TriggerEntity":
                            entity = new TriggerEntity((Game)game, objmodel, objvect);
                            break;
                        default:
                            entity = null;
                            break;
                    }
                    if (entity != null)
                    {
                        entity.BoundingSphere = ReadSphere(node.SelectSingleNode("BoundingSphere"));
                        scene.AddEntity(entity);
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("XML not properly formatted: Value in object with model "
                        + node.SelectSingleNode("model").InnerText);
                }
            }
        }

        private BoundingSphere ReadSphere(XmlNode node)
        {
            try
            {
                float spherex = (float)Convert.ToDouble(node.SelectSingleNode("sphereposx").InnerText);
                float spherey = (float)Convert.ToDouble(node.SelectSingleNode("sphereposy").InnerText);
                float spherez = (float)Convert.ToDouble(node.SelectSingleNode("sphereposz").InnerText);
                Vector3 spherepos = new Vector3(spherex, spherey, spherez);
                float sphererad = (float)Convert.ToDouble(node.SelectSingleNode("sphereradius").InnerText);
                BoundingSphere newsphere = new BoundingSphere(spherepos, sphererad);
                return newsphere;
            }
            catch (Exception e)
            {
                Console.WriteLine("Bounding sphere reading failed.");
                return new BoundingSphere();
            }
        }

        public Boolean SaveScene(Octree scene, string filename)
        {
            ModelInfo savegeo = scene.getGeometry();
            List<ModelInfo> objects = scene.getDrawableObjects();
            List<Item> items = scene.GetItems();
            List<LogicEntity> entities = scene.GetEntities();
            XmlTextWriter scenesaver = new XmlTextWriter(filename, null);
            scenesaver.Formatting = Formatting.Indented;
            scenesaver.WriteStartDocument();

            scenesaver.WriteStartElement("SceneInfo");
            scenesaver.WriteStartElement("size");
            scenesaver.WriteString(scene.TreeSize.ToString());
            scenesaver.WriteEndElement();

            //Write the world geometry to the file
            SaveGeometry(scenesaver, savegeo);
            //Write objects to the file
            SaveObjects(scenesaver, objects);
            //Write items to the file
            SaveItems(scenesaver, items);
            //Write logic entity to the file
            SaveEntities(scenesaver, entities);
            //Close the Content
            scenesaver.WriteEndElement();

            scenesaver.WriteEndDocument();
            scenesaver.Flush();
            scenesaver.Close();
            return true;
        }

        private void SaveGeometry(XmlTextWriter scenesaver, ModelInfo geometry)
        {
            scenesaver.WriteStartElement("WorldGeometry");

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
            scenesaver.WriteString(geometry.FileName);
            scenesaver.WriteEndElement();
            scenesaver.WriteEndElement();
        }

        private void SaveObjects(XmlTextWriter scenesaver, List<ModelInfo> objects)
        {
            scenesaver.WriteStartElement("Conent");
            foreach (ModelInfo obj in objects)
            {
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
                scenesaver.WriteString(obj.FileName);
                scenesaver.WriteEndElement();
                //Save prop's bounding sphere
                SaveSphere(scenesaver, obj);
                scenesaver.WriteEndElement();

            }
        }

        private void SaveItems(XmlTextWriter scenesaver, List<Item> items)
        {
            ModelInfo obj;
            foreach (Item item in items)
            {
                //Store the item's ModelInfo information
                obj = item.Model;

                scenesaver.WriteStartElement("Item");

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
                scenesaver.WriteString(obj.FileName);
                scenesaver.WriteEndElement();

                //Save what type of item it is based on the exact class type
                Type itemtype = item.GetType();
                scenesaver.WriteStartElement("itemtype");
                scenesaver.WriteString(itemtype.ToString());
                scenesaver.WriteEndElement();

                //Save item's bounding sphere
                SaveSphere(scenesaver, obj);

                scenesaver.WriteEndElement();
            }
        }

        private void SaveEntities(XmlTextWriter scenesaver, List<LogicEntity> entities)
        {
            ModelInfo obj;
            foreach (LogicEntity entity in entities)
            {
                //Store the item's ModelInfo information
                obj = new ModelInfo(entity.Position, Vector3.Zero, Vector3.Zero, entity.Model, "entity");

                scenesaver.WriteStartElement("Entity");
                
                scenesaver.WriteStartElement("posx");
                scenesaver.WriteString(obj.Position.X.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("posy");
                scenesaver.WriteString(obj.Position.Y.ToString());
                scenesaver.WriteEndElement();
                scenesaver.WriteStartElement("posz");
                scenesaver.WriteString(obj.Position.Z.ToString());
                scenesaver.WriteEndElement();

                scenesaver.WriteStartElement("model");
                scenesaver.WriteString(obj.FileName);
                scenesaver.WriteEndElement();

                //Save what type of item it is based on the exact class type
                Type entitytype = entity.GetType();
                scenesaver.WriteStartElement("entitytype");
                scenesaver.WriteString(entitytype.ToString());
                scenesaver.WriteEndElement();

                //Write the entity's bounding sphere
                SaveSphere(scenesaver, obj);

                scenesaver.WriteEndElement();
            }
        }

        private void SaveSphere(XmlTextWriter scenesaver, ModelInfo obj)
        {
            scenesaver.WriteStartElement("BoundingSphere");

            scenesaver.WriteStartElement("sphereposx");
            scenesaver.WriteString(obj.BoundingSphere.Center.X.ToString());
            scenesaver.WriteEndElement();
            scenesaver.WriteStartElement("sphereposy");
            scenesaver.WriteString(obj.BoundingSphere.Center.Y.ToString());
            scenesaver.WriteEndElement();
            scenesaver.WriteStartElement("sphereposz");
            scenesaver.WriteString(obj.BoundingSphere.Center.Z.ToString());
            scenesaver.WriteEndElement();

            scenesaver.WriteStartElement("sphereradius");
            scenesaver.WriteString(obj.BoundingSphere.Radius.ToString());
            scenesaver.WriteEndElement();

            scenesaver.WriteEndElement();
        }
    }
}