using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000359 RID: 857
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x06001A52 RID: 6738 RVA: 0x0009D6F8 File Offset: 0x0009B8F8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool flag = false;
			for (int i = 0; i < this.mods.Count; i++)
			{
				if (ModLister.HasActiveModWithName(this.mods[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return true;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0009D762 File Offset: 0x0009B962
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false, false));
		}

		// Token: 0x0400156B RID: 5483
		private List<string> mods;

		// Token: 0x0400156C RID: 5484
		private PatchOperation match;

		// Token: 0x0400156D RID: 5485
		private PatchOperation nomatch;
	}
}
