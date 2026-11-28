using System;

namespace RimWorld
{
	// Token: 0x02001E69 RID: 7785
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAnyOfAttribute : Attribute
	{
		// Token: 0x0600CA22 RID: 51746 RVA: 0x004A589C File Offset: 0x004A3A9C
		public MayRequireAnyOfAttribute(string modId)
		{
			this.modIds = modId.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < this.modIds.Length; i++)
			{
				this.modIds[i] = this.modIds[i].Trim();
			}
		}

		// Token: 0x040072F3 RID: 29427
		public string[] modIds;
	}
}
