using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B82 RID: 2946
	public class JoyGiver_ViewArt : JoyGiver
	{
		// Token: 0x06005588 RID: 21896 RVA: 0x001FD008 File Offset: 0x001FB208
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			Job result;
			try
			{
				JoyGiver_ViewArt.candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where(delegate(Thing thing)
				{
					if (!base.<TryGiveJob>g__Validator|0(thing))
					{
						return false;
					}
					CompArt compArt = thing.TryGetComp<CompArt>();
					if (compArt == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
						defaultInterpolatedStringHandler.AppendLiteral("No CompArt on thing being considered for viewing: ");
						defaultInterpolatedStringHandler.AppendFormatted<Thing>(thing);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						return false;
					}
					if (!compArt.CanShowArt || !compArt.Props.canBeEnjoyedAsArt)
					{
						return false;
					}
					Room room = thing.GetRoom(RegionType.Set_All);
					if (room == null)
					{
						return false;
					}
					if (room.Role.avoidViewingArtIfUnowned)
					{
						Pawn_Ownership ownership = pawn.ownership;
						if (((ownership != null) ? ownership.OwnedRoom : null) == null || pawn.ownership.OwnedRoom != room)
						{
							return false;
						}
					}
					return true;
				}));
				Thing t;
				if (!JoyGiver_ViewArt.candidates.TryRandomElementByWeight((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty, true, -1), 0.5f), out t))
				{
					result = null;
				}
				else
				{
					result = JobMaker.MakeJob(this.def.jobDef, t);
				}
			}
			finally
			{
				JoyGiver_ViewArt.candidates.Clear();
			}
			return result;
		}

		// Token: 0x040032E4 RID: 13028
		private static readonly List<Thing> candidates = new List<Thing>();
	}
}
