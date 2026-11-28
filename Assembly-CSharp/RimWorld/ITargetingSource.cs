using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8D RID: 7565
	public interface ITargetingSource
	{
		// Token: 0x0600C43B RID: 50235
		bool CanHitTarget(LocalTargetInfo target);

		// Token: 0x0600C43C RID: 50236
		bool ValidateTarget(LocalTargetInfo target, bool showMessages = true);

		// Token: 0x0600C43D RID: 50237
		void DrawHighlight(LocalTargetInfo target);

		// Token: 0x0600C43E RID: 50238
		void OrderForceTarget(LocalTargetInfo target);

		// Token: 0x0600C43F RID: 50239
		void OnGUI(LocalTargetInfo target);

		// Token: 0x1700246A RID: 9322
		// (get) Token: 0x0600C440 RID: 50240
		bool CasterIsPawn { get; }

		// Token: 0x1700246B RID: 9323
		// (get) Token: 0x0600C441 RID: 50241
		bool IsMeleeAttack { get; }

		// Token: 0x1700246C RID: 9324
		// (get) Token: 0x0600C442 RID: 50242
		bool Targetable { get; }

		// Token: 0x1700246D RID: 9325
		// (get) Token: 0x0600C443 RID: 50243
		bool MultiSelect { get; }

		// Token: 0x1700246E RID: 9326
		// (get) Token: 0x0600C444 RID: 50244
		bool HidePawnTooltips { get; }

		// Token: 0x1700246F RID: 9327
		// (get) Token: 0x0600C445 RID: 50245
		Thing Caster { get; }

		// Token: 0x17002470 RID: 9328
		// (get) Token: 0x0600C446 RID: 50246
		Pawn CasterPawn { get; }

		// Token: 0x17002471 RID: 9329
		// (get) Token: 0x0600C447 RID: 50247
		Verb GetVerb { get; }

		// Token: 0x17002472 RID: 9330
		// (get) Token: 0x0600C448 RID: 50248
		Texture2D UIIcon { get; }

		// Token: 0x17002473 RID: 9331
		// (get) Token: 0x0600C449 RID: 50249
		TargetingParameters targetParams { get; }

		// Token: 0x17002474 RID: 9332
		// (get) Token: 0x0600C44A RID: 50250
		ITargetingSource DestinationSelector { get; }
	}
}
