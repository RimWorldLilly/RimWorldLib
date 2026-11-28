using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x0200040D RID: 1037
	public class HediffComp_AttachPoints : HediffComp
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x000BC147 File Offset: 0x000BA347
		public HediffCompProperties_AttachPoints Props
		{
			get
			{
				return (HediffCompProperties_AttachPoints)this.props;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x000BC154 File Offset: 0x000BA354
		public AttachPointTracker Points
		{
			get
			{
				if (this.parent.pawn.story == null)
				{
					return null;
				}
				if (this.points == null)
				{
					if (this.parent.pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
					{
						Pawn_StoryTracker story = this.parent.pawn.story;
						bool flag;
						if (story == null)
						{
							flag = (null != null);
						}
						else
						{
							BodyTypeDef bodyType = story.bodyType;
							flag = (((bodyType != null) ? bodyType.attachPointsDessicated : null) != null);
						}
						if (!flag)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(135, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Pawn ");
							defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.parent.pawn);
							defaultInterpolatedStringHandler.AppendLiteral(" is dessicated but their ");
							Pawn_StoryTracker story2 = this.parent.pawn.story;
							defaultInterpolatedStringHandler.AppendFormatted<BodyTypeDef>((story2 != null) ? story2.bodyType : null);
							defaultInterpolatedStringHandler.AppendLiteral(" bodyType def doesn't declare attachPointsDessicated, falling back to non-dessicated attachpoints instead");
							Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					if (this.parent.pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
					{
						Pawn_StoryTracker story3 = this.parent.pawn.story;
						bool flag2;
						if (story3 == null)
						{
							flag2 = (null != null);
						}
						else
						{
							BodyTypeDef bodyType2 = story3.bodyType;
							flag2 = (((bodyType2 != null) ? bodyType2.attachPointsDessicated : null) != null);
						}
						if (flag2)
						{
							this.points = new AttachPointTracker(this.parent.pawn.story.bodyType.attachPointsDessicated, this.parent.pawn);
							goto IL_1AE;
						}
					}
					Pawn_StoryTracker story4 = this.parent.pawn.story;
					bool flag3;
					if (story4 == null)
					{
						flag3 = (null != null);
					}
					else
					{
						BodyTypeDef bodyType3 = story4.bodyType;
						flag3 = (((bodyType3 != null) ? bodyType3.attachPoints : null) != null);
					}
					if (flag3)
					{
						this.points = new AttachPointTracker(this.parent.pawn.story.bodyType.attachPoints, this.parent.pawn);
					}
				}
				IL_1AE:
				return this.points;
			}
		}

		// Token: 0x04001812 RID: 6162
		private AttachPointTracker points;
	}
}
