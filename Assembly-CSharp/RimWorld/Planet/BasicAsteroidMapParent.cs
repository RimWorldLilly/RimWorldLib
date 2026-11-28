using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D1 RID: 9169
	public class BasicAsteroidMapParent : SpaceMapParent
	{
		// Token: 0x0600E481 RID: 58497 RVA: 0x0054E9DC File Offset: 0x0054CBDC
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.preciousResource != null)
			{
				text += "\n" + "TracesOfPreciousResource".Translate(this.preciousResource.Named("RESOURCE"));
			}
			return text.Trim();
		}
	}
}
