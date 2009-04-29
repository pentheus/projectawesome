using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AwesomeEngine
{
    class XMLReader
    {
        //Read an xml file to load a scene
        public Octree loadScene()
        {
            XmlDocument scenedoc = loadXMLFile();
            Octree scene = new Octree(3);
            //World Geometry
            ModelInfo geometry = new ModelInfo();
            //TODO: Read geometry from the xml file
            scene.addgeometry(geometry);


            //Models
            //Read each model from the XMLDocument
            scenedoc.

            //AI scripts, triggers
            return scene;
        }

        //Load the xml file and return it to the method that requested it
        public XmlDocument loadXMLFile()
        {
            String filename = "newfile.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            return xmlDoc;
        }
    }
}