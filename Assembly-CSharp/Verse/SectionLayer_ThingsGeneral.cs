using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C6 RID: 710
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x00081150 File Offset: 0x0007F350
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00081170 File Offset: 0x0007F370
		protected override void TakePrintFrom(Thing t)
		{
			try
			{
				t.Print(this);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception printing ";
				array[1] = ((t != null) ? t.ToString() : null);
				array[2] = " at ";
				array[3] = t.Position.ToString();
				array[4] = ": ";
				int num = 5;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
		}
	}
}
