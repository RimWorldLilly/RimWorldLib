using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF9 RID: 4089
	public class BodyTypeGraphicData
	{
		// Token: 0x06006732 RID: 26418 RVA: 0x00248D3D File Offset: 0x00246F3D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "bodyType", xmlRoot.Name, null, null, null);
			this.texturePath = (xmlRoot.HasChildNodes ? ParseHelper.FromString<string>(xmlRoot.FirstChild.Value) : null);
		}

		// Token: 0x040040E9 RID: 16617
		public BodyTypeDef bodyType;

		// Token: 0x040040EA RID: 16618
		public string texturePath;
	}
}
