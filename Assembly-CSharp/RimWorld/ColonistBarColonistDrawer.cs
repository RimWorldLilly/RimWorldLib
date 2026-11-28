using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C28 RID: 7208
	[StaticConstructorOnStartup]
	public class ColonistBarColonistDrawer
	{
		// Token: 0x17002133 RID: 8499
		// (get) Token: 0x0600B7DE RID: 47070 RVA: 0x0040DC37 File Offset: 0x0040BE37
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x0600B7DF RID: 47071 RVA: 0x0040DC40 File Offset: 0x0040BE40
		public void DrawColonist(Rect rect, Pawn colonist, Map pawnMap, bool highlight, bool reordering)
		{
			float num = this.ColonistBar.GetEntryRectAlpha(rect);
			object obj;
			if (Prefs.VisibleMood)
			{
				Pawn_NeedsTracker needs = colonist.needs;
				if (((needs != null) ? needs.mood : null) != null && colonist.mindState.mentalBreaker.CanDoRandomMentalBreaks && !colonist.Dead)
				{
					obj = !colonist.Downed;
					goto IL_4E;
				}
			}
			obj = 0;
			IL_4E:
			MoodThreshold moodThreshold = MoodThresholdExtensions.CurrentMoodThresholdFor(colonist);
			Color color = moodThreshold.GetColor();
			color.a *= num;
			this.ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref num);
			if (reordering)
			{
				num *= 0.5f;
			}
			Color color2 = new Color(1f, 1f, 1f, num);
			GUI.color = color2;
			object obj2 = obj;
			if (obj2 != null && num >= 1f)
			{
				float num2 = moodThreshold.EdgeExpansion();
				if (num2 > 0f)
				{
					GUI.color = color;
					Widgets.DrawAtlas(rect.ExpandedBy(num2), ColonistBarColonistDrawer.MoodAtlas);
					GUI.color = color2;
				}
			}
			GUI.DrawTexture(rect, ColonistBar.BGTex);
			if (colonist.needs != null && colonist.needs.mood != null)
			{
				Rect position = rect.ContractedBy(2f);
				float num3 = position.height * colonist.needs.mood.CurLevelPercentage;
				position.yMin = position.yMax - num3;
				position.height = num3;
				GUI.DrawTexture(position, ColonistBarColonistDrawer.MoodBGTex);
			}
			if (obj2 != null && num >= 1f)
			{
				float transparency = (moodThreshold < MoodThreshold.Major) ? 0.1f : 0.15f;
				Widgets.DrawBoxSolid(rect, moodThreshold.GetColor().ToTransparent(transparency));
			}
			if (highlight)
			{
				int thickness = (rect.width <= 22f) ? 2 : 3;
				GUI.color = Color.white;
				Widgets.DrawBox(rect, thickness, null);
				GUI.color = color2;
			}
			Rect rect2 = rect.ContractedBy(-2f * this.ColonistBar.Scale);
			if ((colonist.Dead ? Find.Selector.SelectedObjects.Contains(colonist.Corpse) : Find.Selector.SelectedObjects.Contains(colonist)) && !WorldRendererUtility.WorldSelected)
			{
				this.DrawSelectionOverlayOnGUI(colonist, rect2);
			}
			else if (WorldRendererUtility.WorldSelected && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
			{
				this.DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect2);
			}
			GUI.DrawTexture(this.GetPawnTextureRect(rect.position), PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, null, null, false, null));
			if (obj2 != null)
			{
				Rect rect3 = rect.ContractedBy(1f);
				Widgets.BeginGroup(rect3);
				Rect position2 = rect3.AtZero();
				position2.yMin = position2.yMax - 35f;
				GUI.color = color;
				GUI.DrawTexture(position2, ColonistBarColonistDrawer.MoodGradient);
				GUI.color = color2;
				Widgets.EndGroup();
			}
			GUI.color = new Color(1f, 1f, 1f, num * 0.8f);
			this.DrawIcons(rect, colonist);
			GUI.color = color2;
			if (colonist.Dead)
			{
				GUI.DrawTexture(rect, ColonistBarColonistDrawer.DeadColonistTex);
			}
			float num4 = 4f * this.ColonistBar.Scale;
			Vector2 pos = new Vector2(rect.center.x, rect.yMax - num4);
			GenMapUI.DrawPawnLabel(colonist, pos, num, rect.width + this.ColonistBar.SpaceBetweenColonistsHorizontal - 2f, this.pawnLabelsCache, GameFont.Tiny, true, true);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x0600B7E0 RID: 47072 RVA: 0x0040DFB8 File Offset: 0x0040C1B8
		private Rect GroupFrameRect(int group)
		{
			float num = 99999f;
			float num2 = 0f;
			float num3 = 0f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			List<Vector2> drawLocs = this.ColonistBar.DrawLocs;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].group == group)
				{
					num = Mathf.Min(num, drawLocs[i].x);
					num2 = Mathf.Max(num2, drawLocs[i].x + this.ColonistBar.Size.x);
					num3 = Mathf.Max(num3, drawLocs[i].y + this.ColonistBar.Size.y);
				}
			}
			return new Rect(num, 0f, num2 - num, num3 - 0f).ContractedBy(-12f * this.ColonistBar.Scale);
		}

		// Token: 0x0600B7E1 RID: 47073 RVA: 0x0040E0A4 File Offset: 0x0040C2A4
		public void DrawGroupFrame(int group)
		{
			Rect position = this.GroupFrameRect(group);
			Map map = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group).map;
			float num;
			if (map == null)
			{
				if (WorldRendererUtility.WorldSelected)
				{
					num = 1f;
				}
				else
				{
					num = 0.75f;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldSelected)
			{
				num = 0.75f;
			}
			else
			{
				num = 1f;
			}
			Widgets.DrawRectFast(position, new Color(0.5f, 0.5f, 0.5f, 0.4f * num), null);
		}

		// Token: 0x0600B7E2 RID: 47074 RVA: 0x0040E144 File Offset: 0x0040C344
		private void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
		{
			if (map == null)
			{
				if (!WorldRendererUtility.WorldSelected)
				{
					alpha = Mathf.Min(alpha, 0.4f);
					return;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldSelected)
			{
				alpha = Mathf.Min(alpha, 0.4f);
			}
		}

		// Token: 0x0600B7E3 RID: 47075 RVA: 0x0040E17C File Offset: 0x0040C37C
		public void HandleClicks(Rect rect, Pawn colonist, int reorderableGroup, out bool reordering)
		{
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && Event.current.clickCount == 2 && Mouse.IsOver(rect))
			{
				Event.current.Use();
				CameraJumper.TryJump(colonist, CameraJumper.MovementMode.Pan);
			}
			reordering = ReorderableWidget.Reorderable(reorderableGroup, rect, true, true);
			if (Event.current.type == EventType.mouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600B7E4 RID: 47076 RVA: 0x0040E204 File Offset: 0x0040C404
		public void HandleGroupFrameClicks(int group)
		{
			Rect rect = this.GroupFrameRect(group);
			if (Event.current.type == EventType.mouseUp && Event.current.button == 0 && Mouse.IsOver(rect) && !this.ColonistBar.AnyColonistOrCorpseAt(UI.MousePositionOnUIInverted))
			{
				bool worldSelected = WorldRendererUtility.WorldSelected;
				if ((!worldSelected && !Find.Selector.dragBox.IsValidAndActive) || (worldSelected && !Find.WorldSelector.dragBox.IsValidAndActive))
				{
					Find.Selector.dragBox.active = false;
					Find.WorldSelector.dragBox.active = false;
					ColonistBar.Entry entry = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group);
					Map map = entry.map;
					if (map == null)
					{
						if (WorldRendererUtility.WorldSelected)
						{
							CameraJumper.TrySelect(entry.pawn, false);
						}
						else
						{
							CameraJumper.TryJumpAndSelect(entry.pawn, CameraJumper.MovementMode.Pan);
						}
					}
					else
					{
						if (!CameraJumper.TryHideWorld() && Find.CurrentMap != map)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
						Current.Game.CurrentMap = map;
					}
				}
			}
			if (Event.current.type == EventType.mouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600B7E5 RID: 47077 RVA: 0x0040E366 File Offset: 0x0040C566
		public void Notify_RecachedEntries()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x0040E373 File Offset: 0x0040C573
		public void ClearLabelCache()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x0600B7E7 RID: 47079 RVA: 0x0040E380 File Offset: 0x0040C580
		public Rect GetPawnTextureRect(Vector2 pos)
		{
			float x = pos.x;
			float y = pos.y;
			Vector2 vector = ColonistBarColonistDrawer.PawnTextureSize * this.ColonistBar.Scale;
			return new Rect(x + 1f, y - (vector.y - this.ColonistBar.Size.y) - 1f, vector.x, vector.y).ContractedBy(1f);
		}

		// Token: 0x0600B7E8 RID: 47080 RVA: 0x0040E3F4 File Offset: 0x0040C5F4
		private void DrawIcons(Rect rect, Pawn colonist)
		{
			if (colonist.Dead)
			{
				return;
			}
			ColonistBarColonistDrawer.tmpIconsToDraw.Clear();
			bool flag = false;
			if (colonist.CurJob != null)
			{
				JobDef def = colonist.CurJob.def;
				if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
				{
					flag = true;
				}
				else if (def == JobDefOf.Wait_Combat)
				{
					Stance_Busy stance_Busy = colonist.stances.curStance as Stance_Busy;
					if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
					{
						flag = true;
					}
				}
			}
			if (colonist.IsFormingCaravan())
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_FormingCaravan, "ActivityIconFormingCaravan".Translate(), null));
			}
			if (colonist.InAggroMentalState)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MentalStateAggro, colonist.MentalStateDef.LabelCap, null));
			}
			else if (colonist.InMentalState)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MentalStateNonAggro, colonist.MentalStateDef.LabelCap, null));
			}
			else if (colonist.InBed() && colonist.CurrentBed().Medical)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MedicalRest, "ActivityIconMedicalRest".Translate(), null));
			}
			else
			{
				if (colonist.CurJob == null || !colonist.jobs.curDriver.asleep)
				{
					if (colonist.GetCaravan() != null)
					{
						Pawn_NeedsTracker needs = colonist.needs;
						bool flag2;
						if (needs == null)
						{
							flag2 = false;
						}
						else
						{
							Need_Rest rest = needs.rest;
							flag2 = ((rest != null) ? new bool?(rest.Resting) : null).GetValueOrDefault();
						}
						if (flag2)
						{
							goto IL_1B6;
						}
					}
					if (colonist.CurJob != null && colonist.CurJob.def == JobDefOf.FleeAndCower)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Fleeing, "ActivityIconFleeing".Translate(), null));
						goto IL_2A5;
					}
					if (flag)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Attacking, "ActivityIconAttacking".Translate(), null));
						goto IL_2A5;
					}
					if (colonist.mindState.IsIdle && GenDate.DaysPassed >= 1)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Idle, "ActivityIconIdle".Translate(), null));
						goto IL_2A5;
					}
					goto IL_2A5;
				}
				IL_1B6:
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Sleeping, "ActivityIconSleeping".Translate(), null));
			}
			IL_2A5:
			if (colonist.IsBurning())
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Burning, "ActivityIconBurning".Translate(), null));
			}
			if (colonist.Inspired)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Inspired, colonist.InspirationDef.LabelCap, null));
			}
			if (colonist.IsSlaveOfColony)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(colonist.guest.GetIcon(), null, null));
			}
			else
			{
				bool flag3 = false;
				if (colonist.Ideo != null)
				{
					Ideo ideo = colonist.Ideo;
					Precept_Role role = ideo.GetRole(colonist);
					if (role != null)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(role.Icon, null, new Color?(ideo.Color)));
						flag3 = true;
					}
				}
				if (!flag3)
				{
					Faction faction = null;
					if (colonist.HasExtraMiniFaction(null))
					{
						faction = colonist.GetExtraMiniFaction(null);
					}
					else if (colonist.HasExtraHomeFaction(null))
					{
						faction = colonist.GetExtraHomeFaction(null);
					}
					if (faction != null)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(faction.def.FactionIcon, null, new Color?(faction.Color)));
					}
				}
			}
			float num = Mathf.Min(ColonistBarColonistDrawer.BaseIconAreaWidth / (float)ColonistBarColonistDrawer.tmpIconsToDraw.Count, ColonistBarColonistDrawer.BaseIconMaxSize) * this.ColonistBar.Scale;
			Vector2 vector = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
			foreach (ColonistBarColonistDrawer.IconDrawCall iconDrawCall in ColonistBarColonistDrawer.tmpIconsToDraw)
			{
				GUI.color = (iconDrawCall.color ?? Color.white);
				this.DrawIcon(iconDrawCall.texture, ref vector, num, iconDrawCall.tooltip);
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600B7E9 RID: 47081 RVA: 0x0040E8B4 File Offset: 0x0040CAB4
		private void DrawIcon(Texture2D icon, ref Vector2 pos, float iconSize, string tooltip = null)
		{
			Rect rect = new Rect(pos.x, pos.y, iconSize, iconSize);
			GUI.DrawTexture(rect, icon);
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			pos.x += iconSize;
		}

		// Token: 0x0600B7EA RID: 47082 RVA: 0x0040E8FC File Offset: 0x0040CAFC
		private void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
		{
			Thing target = colonist;
			if (colonist.Dead)
			{
				target = colonist.Corpse;
			}
			SelectionDrawerUtility.DrawSelectionOverlayOnGUI(target, rect, 0.4f * this.ColonistBar.Scale, 20f * this.ColonistBar.Scale);
		}

		// Token: 0x0600B7EB RID: 47083 RVA: 0x0040E943 File Offset: 0x0040CB43
		private void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
		{
			SelectionDrawerUtility.DrawSelectionOverlayOnGUI(caravan, rect, 0.4f * this.ColonistBar.Scale, 20f * this.ColonistBar.Scale);
		}

		// Token: 0x0400696E RID: 26990
		private Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();

		// Token: 0x0400696F RID: 26991
		private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));

		// Token: 0x04006970 RID: 26992
		private static readonly Texture2D MoodAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/SubtleGradient", true);

		// Token: 0x04006971 RID: 26993
		private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist", true);

		// Token: 0x04006972 RID: 26994
		private static readonly Texture2D Icon_FormingCaravan = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/FormingCaravan", true);

		// Token: 0x04006973 RID: 26995
		private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x04006974 RID: 26996
		private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);

		// Token: 0x04006975 RID: 26997
		private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest", true);

		// Token: 0x04006976 RID: 26998
		private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x04006977 RID: 26999
		private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing", true);

		// Token: 0x04006978 RID: 27000
		private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking", true);

		// Token: 0x04006979 RID: 27001
		private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle", true);

		// Token: 0x0400697A RID: 27002
		private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning", true);

		// Token: 0x0400697B RID: 27003
		private static readonly Texture2D Icon_Inspired = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Inspired", true);

		// Token: 0x0400697C RID: 27004
		private static readonly Texture2D MoodGradient = ContentFinder<Texture2D>.Get("UI/Widgets/MoodGradient", true);

		// Token: 0x0400697D RID: 27005
		public static readonly Vector2 PawnTextureSize = new Vector2(ColonistBar.BaseSize.x - 2f, 75f);

		// Token: 0x0400697E RID: 27006
		public static readonly Vector3 PawnTextureCameraOffset = new Vector3(0f, 0f, 0.3f);

		// Token: 0x0400697F RID: 27007
		public const float PawnTextureCameraZoom = 1.28205f;

		// Token: 0x04006980 RID: 27008
		private const float PawnTextureHorizontalPadding = 1f;

		// Token: 0x04006981 RID: 27009
		private static readonly float BaseIconAreaWidth = ColonistBarColonistDrawer.PawnTextureSize.x;

		// Token: 0x04006982 RID: 27010
		private static readonly float BaseIconMaxSize = 20f;

		// Token: 0x04006983 RID: 27011
		private const float BaseGroupFrameMargin = 12f;

		// Token: 0x04006984 RID: 27012
		public const float DoubleClickTime = 0.5f;

		// Token: 0x04006985 RID: 27013
		public const float FactionIconSpacing = 2f;

		// Token: 0x04006986 RID: 27014
		public const float IdeoRoleIconSpacing = 2f;

		// Token: 0x04006987 RID: 27015
		public const float SlaveIconSpacing = 2f;

		// Token: 0x04006988 RID: 27016
		private const float MoodGradientHeight = 35f;

		// Token: 0x04006989 RID: 27017
		private static List<ColonistBarColonistDrawer.IconDrawCall> tmpIconsToDraw = new List<ColonistBarColonistDrawer.IconDrawCall>();

		// Token: 0x020038CE RID: 14542
		private struct IconDrawCall
		{
			// Token: 0x06014B59 RID: 84825 RVA: 0x006A0E2B File Offset: 0x0069F02B
			public IconDrawCall(Texture2D texture, string tooltip = null, Color? color = null)
			{
				this.texture = texture;
				this.tooltip = tooltip;
				this.color = color;
			}

			// Token: 0x0400E2D9 RID: 58073
			public Texture2D texture;

			// Token: 0x0400E2DA RID: 58074
			public string tooltip;

			// Token: 0x0400E2DB RID: 58075
			public Color? color;
		}
	}
}
