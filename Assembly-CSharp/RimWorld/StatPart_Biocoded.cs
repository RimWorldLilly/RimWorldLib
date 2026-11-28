using System;

namespace RimWorld
{
	// Token: 0x02001F70 RID: 8048
	public class StatPart_Biocoded : StatPart
	{
		// Token: 0x0600CCE8 RID: 52456 RVA: 0x004B6825 File Offset: 0x004B4A25
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && CompBiocodable.IsBiocoded(req.Thing))
			{
				val *= 0f;
			}
		}

		// Token: 0x0600CCE9 RID: 52457 RVA: 0x004B6848 File Offset: 0x004B4A48
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
