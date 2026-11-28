using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6A RID: 7274
	public static class BuildDesignatorUtility
	{
		// Token: 0x0600B9AD RID: 47533 RVA: 0x00418248 File Offset: 0x00416448
		public static void TryDrawPowerGridAndAnticipatedConnection(BuildableDef def, Rot4 rotation)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && (thingDef.EverTransmitsPower || thingDef.ConnectToPower))
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
				if (thingDef.ConnectToPower)
				{
					IntVec3 intVec = UI.MouseCell();
					IntVec3 intVec2 = intVec;
					if (thingDef.building.isAttachment)
					{
						Thing wallAttachedTo = GenConstruct.GetWallAttachedTo(intVec, rotation, Find.CurrentMap);
						if (wallAttachedTo != null)
						{
							intVec2 = wallAttachedTo.Position;
						}
					}
					CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(intVec2, Find.CurrentMap, null);
					if (compPower != null && !compPower.parent.Position.Fogged(compPower.parent.Map))
					{
						PowerNetGraphics.RenderAnticipatedWirePieceConnecting(intVec2, rotation, def.Size, compPower.parent);
					}
				}
			}
		}
	}
}
