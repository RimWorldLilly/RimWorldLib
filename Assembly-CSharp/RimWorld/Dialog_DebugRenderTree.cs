using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA5 RID: 7333
	public class Dialog_DebugRenderTree : Window
	{
		// Token: 0x17002223 RID: 8739
		// (get) Token: 0x0600BBC4 RID: 48068 RVA: 0x0042BCB9 File Offset: 0x00429EB9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 600f);
			}
		}

		// Token: 0x17002224 RID: 8740
		// (get) Token: 0x0600BBC5 RID: 48069 RVA: 0x0042BCCA File Offset: 0x00429ECA
		protected override float Margin
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x0600BBC6 RID: 48070 RVA: 0x0042BCD4 File Offset: 0x00429ED4
		public Dialog_DebugRenderTree(Pawn pawn) : base(null)
		{
			this.doCloseX = true;
			this.preventCameraMotion = false;
			this.closeOnAccept = false;
			this.draggable = true;
			this.Init(pawn);
		}

		// Token: 0x0600BBC7 RID: 48071 RVA: 0x0042BD24 File Offset: 0x00429F24
		public void Init(Pawn pawn)
		{
			this.currentNode = null;
			this.scrollPosition = Vector2.zero;
			this.scrollHeight = 0f;
			this.nodeExpanded.Clear();
			this.pawn = pawn;
			this.tree = pawn.Drawer.renderer.renderTree;
			this.currentAnimation = this.tree.currentAnimation;
			this.optionalTitle = pawn.LabelShortCap + " (" + pawn.RaceProps.renderTree.defName + ")";
			this.drawParms = new PawnDrawParms
			{
				pawn = pawn,
				rotDrawMode = RotDrawMode.Fresh,
				flags = (PawnRenderFlags.Headgear | PawnRenderFlags.Clothes)
			};
			PawnRenderNode rootNode = this.tree.rootNode;
			if (rootNode == null)
			{
				return;
			}
			this.AddNode(rootNode, null);
		}

		// Token: 0x0600BBC8 RID: 48072 RVA: 0x0042BDF4 File Offset: 0x00429FF4
		private void AddNode(PawnRenderNode node, PawnRenderNode parent)
		{
			if (parent != null && !this.nodeExpanded.ContainsKey(parent))
			{
				this.nodeExpanded.Add(parent, true);
			}
			if (!node.children.NullOrEmpty<PawnRenderNode>())
			{
				foreach (PawnRenderNode node2 in node.children)
				{
					this.AddNode(node2, node);
				}
			}
		}

		// Token: 0x0600BBC9 RID: 48073 RVA: 0x0042BE50 File Offset: 0x0042A050
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			Pawn pawn = Find.Selector.SelectedPawns.FirstOrDefault<Pawn>();
			if (pawn != null && pawn != this.pawn)
			{
				this.Init(pawn);
			}
			this.drawParms.facing = this.pawn.Rotation;
			this.drawParms.posture = this.pawn.GetPosture();
			this.drawParms.bed = this.pawn.CurrentBed();
			this.drawParms.coveredInFoam = this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam;
			Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
			this.drawParms.carriedThing = ((carryTracker != null) ? carryTracker.CarriedThing : null);
			this.drawParms.dead = this.pawn.Dead;
			this.drawParms.rotDrawMode = this.pawn.Drawer.renderer.CurRotDrawMode;
		}

		// Token: 0x0600BBCA RID: 48074 RVA: 0x0042BF45 File Offset: 0x0042A145
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			this.LeftRect(inRect.LeftHalf());
			this.RightRect(inRect.RightHalf());
		}

		// Token: 0x0600BBCB RID: 48075 RVA: 0x0042BF68 File Offset: 0x0042A168
		private void LeftRect(Rect inRect)
		{
			if (this.pawn == null || this.tree == null || !this.tree.Resolved)
			{
				this.Close(true);
				return;
			}
			Widgets.DrawMenuSection(inRect);
			inRect = inRect.ContractedBy(this.Margin / 2f);
			Widgets.HorizontalSlider(new Rect(inRect.x + this.Margin, inRect.y, inRect.width - this.Margin * 2f, 25f), ref this.alpha, FloatRange.ZeroToOne, "Alpha " + this.alpha.ToStringPercent(), 0.01f);
			this.pawn.Drawer.renderer.renderTree.debugTint = new Color?(Color.white.ToTransparent(this.alpha));
			inRect.yMin += 25f + this.Margin;
			Rect rect = new Rect(inRect.x + this.Margin, inRect.y, "Animation".GetWidthCached() + 4f, 25f);
			using (new TextBlock(GameFont.Tiny, TextAnchor.MiddleLeft))
			{
				Widgets.Label(rect, "Animation");
			}
			Rect rect2 = new Rect(rect.xMax + 4f, rect.y, inRect.width - rect.width - this.Margin * 2f, 25f);
			Widgets.DrawLightHighlight(rect2);
			Widgets.DrawHighlightIfMouseover(rect2);
			using (new TextBlock(TextAnchor.MiddleCenter))
			{
				Widgets.Label(rect2, (this.currentAnimation == null) ? "None" : this.currentAnimation.defName);
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("None", delegate()
				{
					this.currentAnimation = null;
					this.pawn.Drawer.renderer.SetAnimation(null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (AnimationDef animationDef in DefDatabase<AnimationDef>.AllDefsListForReading)
				{
					AnimationDef def = animationDef;
					list.Add(new FloatMenuOption(animationDef.defName, delegate()
					{
						this.currentAnimation = def;
						this.pawn.Drawer.renderer.SetAnimation(def);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			inRect.yMin += 25f + this.Margin;
			using (new TextBlock(GameFont.Tiny))
			{
				Widgets.CheckboxLabeled(new Rect(inRect.x + this.Margin, inRect.y, inRect.width - this.Margin * 2f, 25f), "Show all nodes", ref this.showAll, false, null, null, false, false);
			}
			inRect.yMin += 25f + this.Margin;
			Widgets.BeginScrollView(inRect, ref this.scrollPosition, new Rect(0f, 0f, inRect.width - 16f, this.scrollHeight), true);
			float num = 0f;
			this.ListNode(this.tree.rootNode, 0, ref num, inRect.width - 16f);
			if (Event.current.type == EventType.layout)
			{
				this.scrollHeight = num;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600BBCC RID: 48076 RVA: 0x0042C328 File Offset: 0x0042A528
		private void RightRect(Rect inRect)
		{
			Widgets.DrawMenuSection(inRect);
			inRect = inRect.ContractedBy(this.Margin / 2f);
			Widgets.BeginGroup(inRect);
			Rect rect = new Rect(0f, 0f, inRect.width, Text.LineHeight);
			Rect rect2 = new Rect(this.Margin, rect.height + this.Margin, inRect.width - this.Margin * 2f, 30f);
			Widgets.DrawLightHighlight(rect);
			if (this.currentNode != null)
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rect, this.currentNode.ToString().Truncate(inRect.width, null));
				}
				Widgets.Label(rect2, this.currentNode.Worker.GetType().Name.Truncate(inRect.width, null));
				rect2.y += rect2.height;
				bool debugEnabled = this.currentNode.debugEnabled;
				Widgets.CheckboxLabeled(rect2, "Enabled", ref debugEnabled, false, null, null, false, false);
				if (debugEnabled != this.currentNode.debugEnabled)
				{
					this.currentNode.debugEnabled = debugEnabled;
					this.currentNode.requestRecache = true;
				}
				rect2.y += rect2.height;
				Vector3 vector2;
				Vector3 vector = this.currentNode.Worker.OffsetFor(this.currentNode, this.drawParms, out vector2);
				vector.y = this.currentNode.Worker.AltitudeFor(this.currentNode, this.drawParms);
				float num = this.currentNode.Props.baseLayer;
				if (this.currentNode.Props.drawData != null)
				{
					num = this.currentNode.Props.drawData.LayerForRot(this.drawParms.facing, num);
				}
				num += this.currentNode.debugLayerOffset;
				Widgets.Label(rect2, "Offset " + vector.ToString("F2") + " Layer " + num.ToString());
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugOffset.x, Dialog_DebugRenderTree.OffsetRange, "X: " + this.currentNode.debugOffset.x.ToString(), 0.05f);
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugOffset.z, Dialog_DebugRenderTree.OffsetRange, "Z: " + this.currentNode.debugOffset.z.ToString(), 0.05f);
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugLayerOffset, Dialog_DebugRenderTree.LayerRange, "Layer: " + this.currentNode.debugLayerOffset.ToString(), 1f);
				rect2.y += rect2.height;
				rect2.y += this.Margin;
				Rect rect3 = rect2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Pivot (");
				defaultInterpolatedStringHandler.AppendFormatted(vector2.x.ToStringPercent());
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(vector2.z.ToStringPercent());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				Widgets.Label(rect3, defaultInterpolatedStringHandler.ToStringAndClear());
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugPivotOffset.x, FloatRange.ZeroToOne, "X: " + this.currentNode.debugPivotOffset.x.ToStringPercent(), 0.01f);
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugPivotOffset.y, FloatRange.ZeroToOne, "Y: " + this.currentNode.debugPivotOffset.y.ToStringPercent(), 0.01f);
				rect2.y += rect2.height;
				rect2.y += this.Margin;
				float y = this.currentNode.Worker.RotationFor(this.currentNode, this.drawParms).eulerAngles.y;
				Widgets.Label(rect2, "Rotation " + y.ToString("F0") + "°");
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugAngleOffset, Dialog_DebugRenderTree.AngleRange, "Angle: " + this.currentNode.debugAngleOffset.ToString("F0") + "°", 1f);
				rect2.y += rect2.height;
				Vector3 vector3 = this.currentNode.Worker.ScaleFor(this.currentNode, this.drawParms);
				rect2.y += this.Margin;
				Widgets.Label(rect2, "Scale " + vector3.ToString());
				rect2.y += rect2.height;
				Widgets.HorizontalSlider(rect2, ref this.currentNode.debugScale, Dialog_DebugRenderTree.ScaleRange, "Scale: " + this.currentNode.debugScale.ToStringPercent("F0"), 0.01f);
				rect2.y += rect2.height;
				rect2.y += this.Margin;
				bool debugFlip = this.currentNode.debugFlip;
				bool value = this.currentNode.FlipGraphic(this.drawParms);
				Rect rect4 = rect2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Flip (");
				defaultInterpolatedStringHandler2.AppendFormatted<bool>(value);
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				Widgets.CheckboxLabeled(rect4, defaultInterpolatedStringHandler2.ToStringAndClear(), ref debugFlip, false, null, null, false, false);
				if (debugFlip != this.currentNode.debugFlip)
				{
					this.currentNode.debugFlip = debugFlip;
					this.currentNode.requestRecache = true;
				}
				rect2.y += rect2.height;
				if ((this.currentNode.debugAngleOffset != 0f || this.currentNode.debugScale != 1f || this.currentNode.debugOffset != Vector3.zero || this.currentNode.debugLayerOffset != 0f || !this.currentNode.debugEnabled || this.currentNode.debugPivotOffset != DrawData.PivotCenter) && Widgets.ButtonText(new Rect(0f, inRect.height - 25f, inRect.width, 25f), "Reset", true, true, true, null))
				{
					this.currentNode.debugAngleOffset = 0f;
					this.currentNode.debugScale = 1f;
					this.currentNode.debugOffset = Vector3.zero;
					this.currentNode.debugLayerOffset = 0f;
					this.currentNode.debugEnabled = true;
					this.currentNode.requestRecache = true;
					this.currentNode.debugPivotOffset = DrawData.PivotCenter;
				}
			}
			else
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rect, "No node selected");
				}
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BBCD RID: 48077 RVA: 0x0042CB0C File Offset: 0x0042AD0C
		private void ListNode(PawnRenderNode node, int indent, ref float curY, float width)
		{
			if (!this.showAll && !node.Worker.ShouldListOnGraph(node, this.drawParms))
			{
				return;
			}
			Rect rect = new Rect((float)(indent + 1) * 12f, curY, width, 25f)
			{
				xMax = width
			}.ContractedBy(2f);
			Widgets.DrawHighlight(rect);
			Widgets.DrawHighlightIfMouseover(rect);
			if (this.currentNode == node)
			{
				Widgets.DrawHighlight(rect);
			}
			Rect rect2 = new Rect(rect.x, rect.y, rect.height, rect.height);
			Widgets.DrawLightHighlight(rect2);
			if (node.Props.useGraphic)
			{
				Graphic primaryGraphic = node.PrimaryGraphic;
				Texture texture;
				if (primaryGraphic == null)
				{
					texture = null;
				}
				else
				{
					Material material = primaryGraphic.MatAt(Rot4.South, null);
					texture = ((material != null) ? material.mainTexture : null);
				}
				Texture texture2 = texture;
				if (texture2 != null)
				{
					GUI.DrawTexture(rect2, texture2);
				}
			}
			Rect rect3 = new Rect(rect2.xMax + 4f, rect.y, rect.width - rect.height - 4f, rect.height);
			if (!node.Worker.CanDrawNow(node, this.drawParms) || (node.Props.useGraphic && node.PrimaryGraphic == null))
			{
				GUI.color = ColoredText.SubtleGrayColor;
			}
			Widgets.Label(rect3, node.ToString().Truncate(rect3.width, null));
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.currentNode = node;
			}
			if (!node.children.NullOrEmpty<PawnRenderNode>())
			{
				Rect rect4 = new Rect((float)indent * 12f, curY + 6.5f, 12f, 12f);
				Widgets.DrawHighlightIfMouseover(rect4);
				if (Widgets.ButtonImage(rect4, this.nodeExpanded[node] ? TexButton.Minus : TexButton.Plus, true, null))
				{
					this.nodeExpanded[node] = !this.nodeExpanded[node];
				}
			}
			curY += 25f;
			if (!node.children.NullOrEmpty<PawnRenderNode>() && this.nodeExpanded[node])
			{
				foreach (PawnRenderNode node2 in node.children)
				{
					this.ListNode(node2, indent + 1, ref curY, width);
				}
			}
		}

		// Token: 0x0600BBCE RID: 48078 RVA: 0x0042CD50 File Offset: 0x0042AF50
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(5f, 5f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x04006B01 RID: 27393
		public Pawn pawn;

		// Token: 0x04006B02 RID: 27394
		private PawnRenderTree tree;

		// Token: 0x04006B03 RID: 27395
		private Vector2 scrollPosition;

		// Token: 0x04006B04 RID: 27396
		private float scrollHeight;

		// Token: 0x04006B05 RID: 27397
		private PawnRenderNode currentNode;

		// Token: 0x04006B06 RID: 27398
		private float alpha = 1f;

		// Token: 0x04006B07 RID: 27399
		private PawnDrawParms drawParms;

		// Token: 0x04006B08 RID: 27400
		private Dictionary<PawnRenderNode, bool> nodeExpanded = new Dictionary<PawnRenderNode, bool>();

		// Token: 0x04006B09 RID: 27401
		private bool showAll;

		// Token: 0x04006B0A RID: 27402
		private AnimationDef currentAnimation;

		// Token: 0x04006B0B RID: 27403
		private const float NodeHeight = 25f;

		// Token: 0x04006B0C RID: 27404
		private const float IndentWidth = 12f;

		// Token: 0x04006B0D RID: 27405
		private const float IndentHeightOffset = 6.5f;

		// Token: 0x04006B0E RID: 27406
		private static readonly FloatRange AngleRange = new FloatRange(-180f, 180f);

		// Token: 0x04006B0F RID: 27407
		private static readonly FloatRange ScaleRange = new FloatRange(0.01f, 10f);

		// Token: 0x04006B10 RID: 27408
		private static readonly FloatRange OffsetRange = new FloatRange(-10f, 10f);

		// Token: 0x04006B11 RID: 27409
		private static readonly FloatRange LayerRange = new FloatRange(-100f, 100f);
	}
}
