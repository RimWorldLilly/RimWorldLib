using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013CD RID: 5069
	[StaticConstructorOnStartup]
	public class Command_Ability : Command
	{
		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x06007E2A RID: 32298 RVA: 0x002D1A0C File Offset: 0x002CFC0C
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
		}

		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x06007E2B RID: 32299 RVA: 0x002D1A14 File Offset: 0x002CFC14
		public override Texture2D BGTexture
		{
			get
			{
				return Command_Ability.BGTex;
			}
		}

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06007E2C RID: 32300 RVA: 0x002D1A1B File Offset: 0x002CFC1B
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return Command_Ability.BGTexShrunk;
			}
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x06007E2D RID: 32301 RVA: 0x002D1A22 File Offset: 0x002CFC22
		// (set) Token: 0x06007E2E RID: 32302 RVA: 0x002D1A2A File Offset: 0x002CFC2A
		public Pawn Pawn { get; private set; }

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x06007E2F RID: 32303 RVA: 0x002D1A33 File Offset: 0x002CFC33
		public virtual string Tooltip
		{
			get
			{
				return this.ability.Tooltip;
			}
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x06007E30 RID: 32304 RVA: 0x002D1A40 File Offset: 0x002CFC40
		public override string TopRightLabel
		{
			get
			{
				return this.ability.GizmoExtraLabel;
			}
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x06007E31 RID: 32305 RVA: 0x002D1A4D File Offset: 0x002CFC4D
		// (set) Token: 0x06007E32 RID: 32306 RVA: 0x002D1A5B File Offset: 0x002CFC5B
		public override bool Disabled
		{
			get
			{
				this.DisabledCheck();
				return this.disabled;
			}
			set
			{
				this.disabled = value;
			}
		}

		// Token: 0x06007E33 RID: 32307 RVA: 0x002D1A64 File Offset: 0x002CFC64
		public Command_Ability(Ability ability, Pawn pawn)
		{
			this.ability = ability;
			float num = 5f;
			AbilityCategoryDef category = ability.def.category;
			int? num2 = (category != null) ? new int?(category.displayOrder) : null;
			this.Order = num + ((num2 != null) ? ((float)num2.GetValueOrDefault()) : 0f) / 100f + (float)ability.def.displayOrder / 1000f + (float)ability.def.level / 10000f;
			this.defaultLabel = ability.def.LabelCap;
			this.hotKey = ability.def.hotKey;
			this.icon = ability.def.uiIcon;
			this.shrinkable = true;
			this.Pawn = pawn;
			this.originalLabel = this.defaultLabel;
			string text = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor);
			if (pawn.Name != null)
			{
				string text2 = " (" + pawn.Name.ToStringShort + ")";
				this.pawnLabel = this.defaultLabel + text2;
				this.pawnTooltip = this.Tooltip.Insert(text.Length, text2);
				return;
			}
			this.pawnLabel = this.defaultLabel;
			this.pawnTooltip = this.Tooltip;
		}

		// Token: 0x06007E34 RID: 32308 RVA: 0x002D1BBC File Offset: 0x002CFDBC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			this.defaultLabel = (parms.multipleSelected ? this.pawnLabel : this.originalLabel);
			if (this.devGizmo)
			{
				this.defaultLabel = "DEV: " + this.defaultLabel;
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			bool flag = (this.ability.Casting || KeyBindingDefOf.QueueOrder.IsDownEvent) && !this.ability.CanQueueCast;
			if (flag)
			{
				Widgets.FillableBar(rect, 0f, Command_Ability.cooldownBarTex, null, false);
			}
			else if (this.ability.CooldownTicksRemaining > 0)
			{
				float value = Mathf.InverseLerp((float)this.ability.CooldownTicksTotal, 0f, (float)this.ability.CooldownTicksRemaining);
				Widgets.FillableBar(rect, Mathf.Clamp01(value), Command_Ability.cooldownBarTex, null, false);
				if (this.ability.CooldownTicksRemaining > 0)
				{
					Text.Font = GameFont.Tiny;
					string text = this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false);
					Vector2 vector = Text.CalcSize(text);
					vector.x += 2f;
					Rect rect2 = rect;
					rect2.x = rect.x + rect.width / 2f - vector.x / 2f;
					rect2.width = vector.x;
					rect2.height = vector.y;
					Rect position = rect2.ExpandedBy(8f, 0f);
					Text.Anchor = TextAnchor.UpperCenter;
					GUI.DrawTexture(position, TexUI.GrayTextBG);
					Widgets.Label(rect2, text);
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (result.State == GizmoState.Interacted && this.ability.CanCast && !flag)
			{
				return result;
			}
			return new GizmoResult(result.State);
		}

		// Token: 0x06007E35 RID: 32309 RVA: 0x002D1DA8 File Offset: 0x002CFFA8
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			if (Mouse.IsOver(butRect))
			{
				if (parms.multipleSelected)
				{
					if (this.Pawn.Map != null)
					{
						GenUI.DrawArrowPointingAtWorldspace(this.Pawn.DrawPos, Find.Camera);
					}
					this.defaultDesc = this.pawnTooltip;
				}
				else
				{
					this.defaultDesc = this.Tooltip;
				}
			}
			this.DisabledCheck();
			return base.GizmoOnGUIInt(butRect, parms);
		}

		// Token: 0x06007E36 RID: 32310 RVA: 0x002D1E10 File Offset: 0x002D0010
		public override bool GroupsWith(Gizmo other)
		{
			if (this.ability.def.groupAbility)
			{
				Command_Ability command_Ability = other as Command_Ability;
				if (command_Ability != null)
				{
					return command_Ability.ability.def == this.ability.def;
				}
			}
			return false;
		}

		// Token: 0x06007E37 RID: 32311 RVA: 0x002D1E54 File Offset: 0x002D0054
		public override bool ShowPawnDetailsWith(Gizmo other)
		{
			Command_Ability command_Ability = other as Command_Ability;
			return command_Ability != null && command_Ability.ability.def == this.ability.def;
		}

		// Token: 0x06007E38 RID: 32312 RVA: 0x002D1E88 File Offset: 0x002D0088
		public virtual void GroupAbilityCommands(List<Gizmo> group)
		{
			if (this.groupedCasts == null)
			{
				this.groupedCasts = new List<Command_Ability>();
			}
			this.groupedCasts.Clear();
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].GroupsWith(this))
				{
					this.groupedCasts.Add((Command_Ability)group[i]);
				}
			}
		}

		// Token: 0x06007E39 RID: 32313 RVA: 0x002D1EEC File Offset: 0x002D00EC
		protected virtual void DisabledCheck()
		{
			string str;
			this.disabled = this.ability.GizmoDisabled(out str);
			if (this.disabled)
			{
				this.DisableWithReason(str.CapitalizeFirst());
			}
		}

		// Token: 0x06007E3A RID: 32314 RVA: 0x002D1F20 File Offset: 0x002D0120
		private ITargetingSource GetBetterTargetingSource(LocalTargetInfo t)
		{
			this.groupedCasts.SortBy((Command_Ability c) => c.ability.pawn.Position.DistanceToSquared(t.Cell));
			for (int i = 0; i < this.groupedCasts.Count; i++)
			{
				if (this.groupedCasts[i].ability.CanQueueCast && this.groupedCasts[i].ability.verb.ValidateTarget(t, false))
				{
					return this.groupedCasts[i].ability.verb;
				}
			}
			return null;
		}

		// Token: 0x06007E3B RID: 32315 RVA: 0x002D1FBC File Offset: 0x002D01BC
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			if (!this.ability.def.targetRequired)
			{
				this.ability.QueueCastingJob(this.ability.pawn, LocalTargetInfo.Invalid);
				return;
			}
			Find.DesignatorManager.Deselect();
			if (this.ability.def.targetWorldCell)
			{
				CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.ability.pawn), CameraJumper.MovementMode.Pan);
				Find.WorldTargeter.BeginTargeting(delegate(GlobalTargetInfo t)
				{
					if (this.ability.ValidateGlobalTarget(t))
					{
						this.ability.QueueCastingJob(t);
						return true;
					}
					return false;
				}, true, this.ability.def.uiIcon, !this.ability.pawn.IsCaravanMember(), null, new Func<GlobalTargetInfo, TaggedString>(this.ability.WorldMapExtraLabel), new Func<GlobalTargetInfo, bool>(this.ability.ValidateGlobalTarget), null, false);
				return;
			}
			if (this.groupedCasts.NullOrEmpty<Command_Ability>())
			{
				Find.Targeter.BeginTargeting(this.ability.verb, null, false, null, null, true);
				return;
			}
			Find.Targeter.BeginTargeting(this.ability.verb, null, false, new Func<LocalTargetInfo, ITargetingSource>(this.GetBetterTargetingSource), null, true);
		}

		// Token: 0x06007E3C RID: 32316 RVA: 0x002D2100 File Offset: 0x002D0300
		public override void GizmoUpdateOnMouseover()
		{
			Verb_CastAbility verb_CastAbility = this.ability.verb as Verb_CastAbility;
			if (verb_CastAbility != null)
			{
				verb_CastAbility.verbProps.DrawRadiusRing(verb_CastAbility.caster.Position, verb_CastAbility);
			}
			this.ability.OnGizmoUpdate();
		}

		// Token: 0x06007E3D RID: 32317 RVA: 0x002D2143 File Offset: 0x002D0343
		protected void DisableWithReason(string reason)
		{
			this.disabledReason = reason;
			this.disabled = true;
		}

		// Token: 0x04004F33 RID: 20275
		protected Ability ability;

		// Token: 0x04004F34 RID: 20276
		private List<Command_Ability> groupedCasts;

		// Token: 0x04004F35 RID: 20277
		private string pawnLabel;

		// Token: 0x04004F36 RID: 20278
		private string originalLabel;

		// Token: 0x04004F37 RID: 20279
		private string pawnTooltip;

		// Token: 0x04004F38 RID: 20280
		public bool devGizmo;

		// Token: 0x04004F3A RID: 20282
		public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG", true);

		// Token: 0x04004F3B RID: 20283
		public new static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBGShrunk", true);

		// Token: 0x04004F3C RID: 20284
		private static readonly Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
