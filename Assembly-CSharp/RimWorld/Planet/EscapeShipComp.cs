using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002361 RID: 9057
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x0600E080 RID: 57472 RVA: 0x0053798C File Offset: 0x00535B8C
		public override void PostMapGenerate()
		{
			Building_ShipReactor building_ShipReactor = ((MapParent)this.parent).Map.listerBuildings.AllBuildingsColonistOfClass<Building_ShipReactor>().FirstOrDefault<Building_ShipReactor>();
			if (building_ShipReactor != null)
			{
				building_ShipReactor.charlonsReactor = true;
			}
		}

		// Token: 0x0600E081 RID: 57473 RVA: 0x005379C3 File Offset: 0x00535BC3
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			EscapeShipComp.<GetFloatMenuOptions>d__1 <GetFloatMenuOptions>d__ = new EscapeShipComp.<GetFloatMenuOptions>d__1(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			return <GetFloatMenuOptions>d__;
		}
	}
}
