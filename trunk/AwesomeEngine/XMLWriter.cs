using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace AwesomeEngine
{
    class XMLWriter
    {
        //Create an XML file containing information for the scene, and saves it
        //to the disk.
        public void writeScene(Octree scene)
        {
            //Array of child elements to be created from elements of the scene
            List<XmlElement> elements = new List<XmlElement>();
            //Do necessary stuff to create child nodes, then write them
            writeXMLFile(elements);
        }

        //Creates an XML file and writes information to it.
        //This method is called by other case-specific functions, which pass
        //it the List "info" that contains the first level child nodes
        //to be appended to the root node
        void writeXMLFile(List<XmlElement> info)
        {
            try
            {
                //pick whatever filename with .xml extension
                string filename = "XML"+DateTime.Now.Day + ".xml";

                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(filename);
                    //Will probably need to clear file here.
                    xmlDoc.RemoveAll();
                }
                catch(System.IO.FileNotFoundException)
                {
                    //if file is not found, create a new xml file
                    XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                    xmlWriter.WriteStartElement("Scene");
                    //If WriteProcessingInstruction is used as above,
                    //Do not use WriteEndElement() here
                    //xmlWriter.WriteEndElement();
                    //it will cause the &ltRoot></Root> to be &ltRoot />
                    xmlWriter.Close();
                    xmlDoc.Load(filename);
                }
                XmlNode root = xmlDoc.DocumentElement;

                foreach(XmlElement e in info)
                {
                    root.AppendChild(e);
                }

                xmlDoc.Save(filename);
            }
            catch(Exception ex)
            {
                WriteError(ex.ToString());
            }
        }

        void WriteError(string str)
        {
            Console.Write(str);
        }

    }
}