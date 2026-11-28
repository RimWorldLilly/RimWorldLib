using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002373 RID: 9075
	public static class CaravanArrivalActionUtility
	{
		// Token: 0x0600E144 RID: 57668 RVA: 0x00539F77 File Offset: 0x00538177
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Caravan caravan, PlanetTile pathDestination, WorldObject revalidateWorldClickTarget, Action<Action> confirmActionProxy = null) where T : CaravanArrivalAction
		{
			CaravanArrivalActionUtility.<GetFloatMenuOptions>d__0<T> <GetFloatMenuOptions>d__ = new CaravanArrivalActionUtility.<GetFloatMenuOptions>d__0<T>(-2);
			<GetFloatMenuOptions>d__.<>3__acceptanceReportGetter = acceptanceReportGetter;
			<GetFloatMenuOptions>d__.<>3__arrivalActionGetter = arrivalActionGetter;
			<GetFloatMenuOptions>d__.<>3__label = label;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			<GetFloatMenuOptions>d__.<>3__pathDestination = pathDestination;
			<GetFloatMenuOptions>d__.<>3__revalidateWorldClickTarget = revalidateWorldClickTarget;
			<GetFloatMenuOptions>d__.<>3__confirmActionProxy = confirmActionProxy;
			return <GetFloatMenuOptions>d__;
		}
	}
}
