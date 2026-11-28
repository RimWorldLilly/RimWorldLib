using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016AD RID: 5805
	public class RitualObligationTrigger_MemberDied : RitualObligationTrigger
	{
		// Token: 0x06009170 RID: 37232 RVA: 0x003537F8 File Offset: 0x003519F8
		public override void Notify_MemberDied(Pawn p)
		{
			if ((this.mustBePlayerIdeo && !Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || (p.HomeFaction != Faction.OfPlayer && !p.IsSlave) || !p.IsFreeColonist || p.IsKidnapped())
			{
				return;
			}
			this.ritual.AddObligation(new RitualObligation(this.ritual, p, true)
			{
				sendLetter = !p.IsSlave
			});
		}
	}
}
