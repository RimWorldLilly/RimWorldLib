using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020015C1 RID: 5569
	public class Aptitude
	{
		// Token: 0x06008A04 RID: 35332 RVA: 0x0031F1CA File Offset: 0x0031D3CA
		public Aptitude()
		{
		}

		// Token: 0x06008A05 RID: 35333 RVA: 0x0031F1D2 File Offset: 0x0031D3D2
		public Aptitude(SkillDef skill, int level)
		{
			this.skill = skill;
			this.level = level;
		}

		// Token: 0x06008A06 RID: 35334 RVA: 0x0031F1E8 File Offset: 0x0031D3E8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, typeof(SkillDef));
			this.level = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040054F0 RID: 21744
		public SkillDef skill;

		// Token: 0x040054F1 RID: 21745
		public int level;
	}
}
