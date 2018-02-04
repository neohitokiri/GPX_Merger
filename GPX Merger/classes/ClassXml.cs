using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace GPX_Merger.classes
{
	class ClassXml
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlstring"></param>
		/// <returns></returns>
		static public XElement ReadXmlDocument(string xmlstring)
		{
			if (File.Exists(xmlstring))
			{
				var contents = XElement.Load(@xmlstring);

				foreach (var elemento in contents.Elements())
				{
					if (elemento.Name.LocalName.ToLower().Contains("trk"))
						return elemento;
				}
			}
			return new XElement(string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="fileName"></param>
		/// <param name="minify"></param>
		/// <returns></returns>
		static public string createXmlDoc(List<string> content, string fileName, bool minify = false)
		{
			try
			{
				var elementos = new List<XElement>();
				var xmlDeclaration = new XDeclaration("1.0", "UTF-8", null);

				foreach (var item in content)
					elementos.Add(ReadXmlDocument(item));

				var doc = new XDocument(new XElement("gpx", elementos));
				doc.Declaration = xmlDeclaration;
				/*
							if (minify)
							{
								var minifier = new XMLMinifier(XMLMinifierSettings.Aggressive);
								var minified = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>" +
									minifier.Minify(doc.ToString());

								File.WriteAllText(fileName, minified);
							}
							else
				*/
				doc.Save(fileName);
				return string.Empty;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}
	}
}
