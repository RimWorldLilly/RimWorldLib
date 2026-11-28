using System;

namespace RimWorld
{
	// Token: 0x02001E62 RID: 7778
	[AttributeUsage(AttributeTargets.Field)]
	public class DefAliasAttribute : Attribute
	{
		// Token: 0x0600CA1B RID: 51739 RVA: 0x004A583D File Offset: 0x004A3A3D
		public DefAliasAttribute(string defName)
		{
			this.defName = defName;
		}

		// Token: 0x040072F1 RID: 29425
		public string defName;
	}
}
