using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000379 RID: 889
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x06001B28 RID: 6952 RVA: 0x000A7421 File Offset: 0x000A5621
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000A7430 File Offset: 0x000A5630
		public void RenderStatusOverlays(Vector3 offset, Quaternion quat, Mesh headMesh)
		{
			Vector3 vector = this.pawn.DrawPos + offset + new Vector3(0f, 0f, 0.32f);
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (((needs != null) ? needs.mood : null) != null && !this.pawn.Downed)
			{
				if (this.pawn.HitPoints > 0 && this.pawn.IsColonistPlayerControlled)
				{
					if (this.pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
					{
						if (Time.time % 1.2f < 0.4f)
						{
							this.DrawHeadGlow(vector, PawnHeadOverlays.MentalStateImminentMat);
						}
					}
					else if (this.pawn.mindState.mentalBreaker.BreakExtremeIsApproaching && Time.time % 1.2f < 0.4f)
					{
						this.DrawHeadGlow(vector, PawnHeadOverlays.UnhappyMat);
					}
				}
				Pawn_MindState mindState = this.pawn.mindState;
				MentalStateDef mentalStateDef;
				if (mindState == null)
				{
					mentalStateDef = null;
				}
				else
				{
					MentalStateHandler mentalStateHandler = mindState.mentalStateHandler;
					mentalStateDef = ((mentalStateHandler != null) ? mentalStateHandler.CurStateDef : null);
				}
				MentalStateDef mentalStateDef2 = mentalStateDef;
				if ((ModsConfig.OdysseyActive && mentalStateDef2 == MentalStateDefOf.Terror) || (ModsConfig.BiotechActive && mentalStateDef2 == MentalStateDefOf.PanicFleeFire))
				{
					ThingDefOf.Mote_TerrorHalo.graphicData.Graphic.DrawWorker(vector, this.pawn.Rotation, null, null, 0f);
				}
			}
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x000A7584 File Offset: 0x000A5784
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04001644 RID: 5700
		private Pawn pawn;

		// Token: 0x04001645 RID: 5701
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04001646 RID: 5702
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04001647 RID: 5703
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04001648 RID: 5704
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
