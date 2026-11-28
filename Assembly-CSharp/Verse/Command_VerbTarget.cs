using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200060C RID: 1548
	public class Command_VerbTarget : Command
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x00136980 File Offset: 0x00134B80
		public override Color IconDrawColor
		{
			get
			{
				if (this.verb.EquipmentSource != null)
				{
					return this.verb.EquipmentSource.DrawColor;
				}
				return base.IconDrawColor;
			}
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x001369A8 File Offset: 0x00134BA8
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			if (this.ownerThing != null)
			{
				Thing thing = this.ownerThing;
				float alpha = 1f;
				float scale = this.iconDrawScale * 0.85f;
				bool lowLight = parms.lowLight;
				Widgets.ThingIcon(rect, thing, alpha, null, false, scale, lowLight);
				return;
			}
			base.DrawIcon(rect, buttonMat, parms);
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x001369F8 File Offset: 0x00134BF8
		public override void GizmoUpdateOnMouseover()
		{
			if (!this.drawRadius)
			{
				return;
			}
			this.verb.verbProps.DrawRadiusRing(this.verb.caster.Position, this.verb);
			if (!this.groupedVerbs.NullOrEmpty<Verb>())
			{
				foreach (Verb verb in this.groupedVerbs)
				{
					verb.verbProps.DrawRadiusRing(verb.caster.Position, verb);
				}
			}
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x00136A98 File Offset: 0x00134C98
		public override bool GroupsWith(Gizmo other)
		{
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				return false;
			}
			if (this.ownerThing != null && command_VerbTarget.ownerThing != null)
			{
				return this.ownerThing.def == command_VerbTarget.ownerThing.def;
			}
			return base.GroupsWith(other);
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x00136AE4 File Offset: 0x00134CE4
		public override void MergeWith(Gizmo other)
		{
			base.MergeWith(other);
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				Log.ErrorOnce("Tried to merge Command_VerbTarget with unexpected type", 73406263);
				return;
			}
			if (this.groupedVerbs == null)
			{
				this.groupedVerbs = new List<Verb>();
			}
			this.groupedVerbs.Add(command_VerbTarget.verb);
			if (command_VerbTarget.groupedVerbs != null)
			{
				this.groupedVerbs.AddRange(command_VerbTarget.groupedVerbs);
			}
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x00136B50 File Offset: 0x00134D50
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Targeter targeter = Find.Targeter;
			if (this.verb.CasterIsPawn && targeter.targetingSource != null && targeter.targetingSource.GetVerb.verbProps == this.verb.verbProps)
			{
				Pawn casterPawn = this.verb.CasterPawn;
				if (!targeter.IsPawnTargeting(casterPawn))
				{
					targeter.targetingSourceAdditionalPawns.Add(casterPawn);
					return;
				}
			}
			else
			{
				Find.Targeter.BeginTargeting(this.verb, null, false, null, null, this.requiresAvailableVerb);
			}
		}

		// Token: 0x04001F11 RID: 7953
		public Verb verb;

		// Token: 0x04001F12 RID: 7954
		private List<Verb> groupedVerbs;

		// Token: 0x04001F13 RID: 7955
		public Thing ownerThing;

		// Token: 0x04001F14 RID: 7956
		public bool drawRadius = true;

		// Token: 0x04001F15 RID: 7957
		public bool requiresAvailableVerb = true;
	}
}
