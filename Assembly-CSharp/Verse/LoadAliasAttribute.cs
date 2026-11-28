using System;

namespace Verse
{
	// Token: 0x020005EA RID: 1514
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class LoadAliasAttribute : Attribute
	{
		// Token: 0x06002FB0 RID: 12208 RVA: 0x00131AC2 File Offset: 0x0012FCC2
		public LoadAliasAttribute(string alias)
		{
			this.alias = alias;
		}

		// Token: 0x04001E54 RID: 7764
		public string alias;
	}
}
