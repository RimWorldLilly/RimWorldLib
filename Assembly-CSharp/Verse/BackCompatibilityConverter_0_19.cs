using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000685 RID: 1669
	public class BackCompatibilityConverter_0_19 : BackCompatibilityConverter
	{
		// Token: 0x060034CB RID: 13515 RVA: 0x00156600 File Offset: 0x00154800
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 && minorVer <= 19;
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x0015660F File Offset: 0x0015480F
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x00156612 File Offset: 0x00154812
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x00156618 File Offset: 0x00154818
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.foodRestrictionDatabase == null)
				{
					game.foodRestrictionDatabase = new FoodRestrictionDatabase();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.foodRestriction == null && pawn.RaceProps.Humanlike && ((pawn.Faction != null && pawn.Faction.IsPlayer) || (pawn.HostFaction != null && pawn.HostFaction.IsPlayer)))
				{
					pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);
				}
			}
		}
	}
}
