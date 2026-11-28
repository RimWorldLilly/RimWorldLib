using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055E RID: 1374
	public class Graphic_Appearances : Graphic
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060028E0 RID: 10464 RVA: 0x000FB8B8 File Offset: 0x000F9AB8
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index].MatSingle;
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000FB8D0 File Offset: 0x000F9AD0
		private ThingDef StuffOfThing(Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				return constructible.EntityToBuildStuff();
			}
			return thing.Stuff;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000FB8F4 File Offset: 0x000F9AF4
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.SubGraphicFor(thing).MatAt(rot, thing);
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000FB904 File Offset: 0x000F9B04
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<StuffAppearanceDef> allDefsListForReading = DefDatabase<StuffAppearanceDef>.AllDefsListForReading;
			this.subGraphics = new Graphic[allDefsListForReading.Count];
			for (int i = 0; i < this.subGraphics.Length; i++)
			{
				StuffAppearanceDef stuffAppearance = allDefsListForReading[i];
				string text = req.path;
				if (!stuffAppearance.pathPrefix.NullOrEmpty())
				{
					text = stuffAppearance.pathPrefix + "/" + text.Split('/', StringSplitOptions.None).Last<string>();
				}
				Texture2D texture2D = (from x in ContentFinder<Texture2D>.GetAllInFolder(text)
				where x.name.EndsWith(stuffAppearance.defName)
				select x).FirstOrDefault<Texture2D>();
				if (texture2D != null)
				{
					this.subGraphics[i] = GraphicDatabase.Get<Graphic_Single>(text + "/" + texture2D.name, req.shader, this.drawSize, this.color);
				}
			}
			for (int j = 0; j < this.subGraphics.Length; j++)
			{
				if (this.subGraphics[j] == null)
				{
					this.subGraphics[j] = this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index];
				}
			}
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000FBA56 File Offset: 0x000F9C56
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Appearances.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Appearances>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000FBA93 File Offset: 0x000F9C93
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingleFor(thing);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000FBAA2 File Offset: 0x000F9CA2
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.SubGraphicFor(thing).DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000FBAB8 File Offset: 0x000F9CB8
		public Graphic SubGraphicFor(Thing thing)
		{
			StuffAppearanceDef smooth = StuffAppearanceDefOf.Smooth;
			if (thing != null)
			{
				return this.SubGraphicFor(this.StuffOfThing(thing));
			}
			return this.subGraphics[(int)smooth.index];
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000FBAEC File Offset: 0x000F9CEC
		public Graphic SubGraphicFor(ThingDef stuff)
		{
			StuffAppearanceDef app = StuffAppearanceDefOf.Smooth;
			if (stuff != null && stuff.stuffProps.appearance != null)
			{
				app = stuff.stuffProps.appearance;
			}
			return this.SubGraphicFor(app);
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000FBB22 File Offset: 0x000F9D22
		public Graphic SubGraphicFor(StuffAppearanceDef app)
		{
			return this.subGraphics[(int)app.index];
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000FBB34 File Offset: 0x000F9D34
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Appearance(path=",
				this.path,
				", color=",
				this.color.ToString(),
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x04001CB5 RID: 7349
		protected Graphic[] subGraphics;
	}
}
