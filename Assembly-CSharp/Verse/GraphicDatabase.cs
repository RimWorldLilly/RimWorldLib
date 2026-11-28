using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000557 RID: 1367
	public static class GraphicDatabase
	{
		// Token: 0x060028B7 RID: 10423 RVA: 0x000FADC8 File Offset: 0x000F8FC8
		public static Graphic Get<T>(string path) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, ShaderDatabase.Cutout, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000FAE08 File Offset: 0x000F9008
		public static Graphic Get<T>(string path, Shader shader) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000FAE44 File Offset: 0x000F9044
		public static Graphic Get<T>(string path, Shader shader, string mask) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null, mask));
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000FAE80 File Offset: 0x000F9080
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, 0, null, null));
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000FAEB4 File Offset: 0x000F90B4
		public static Graphic Get<T>(string path, Shader shader, string mask, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, renderQueue, null, mask));
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x000FAEF0 File Offset: 0x000F90F0
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000FAF24 File Offset: 0x000F9124
		public static Graphic Get<T>(Texture2D texture, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), texture, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000FAF58 File Offset: 0x000F9158
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, null, 0, null, null));
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000FAF88 File Offset: 0x000F9188
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, string maskPath = null) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, data, 0, null, maskPath));
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000FAFBC File Offset: 0x000F91BC
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, string maskPath = null)
		{
			return GraphicDatabase.Get(graphicClass, path, shader, drawSize, color, colorTwo, null, null, maskPath);
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000FAFDC File Offset: 0x000F91DC
		public static Graphic Get(Type graphicClass, Texture2D texture, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			return GraphicDatabase.Get(new GraphicRequest(graphicClass, texture, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath));
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x000FB004 File Offset: 0x000F9204
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			return GraphicDatabase.Get(new GraphicRequest(graphicClass, path, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath));
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000FB02C File Offset: 0x000F922C
		private static Graphic Get(GraphicRequest req)
		{
			try
			{
				Func<GraphicRequest, Graphic> func;
				if (!GraphicDatabase.cachedGraphicGetters.TryGetValue(req.graphicClass, out func))
				{
					MethodInfo method = typeof(GraphicDatabase).GetMethod("GetInner", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
					{
						req.graphicClass
					});
					func = (Func<GraphicRequest, Graphic>)Delegate.CreateDelegate(typeof(Func<GraphicRequest, Graphic>), method);
					GraphicDatabase.cachedGraphicGetters.Add(req.graphicClass, func);
				}
				return func(req);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception getting ";
				int num = 1;
				Type graphicClass = req.graphicClass;
				array[num] = ((graphicClass != null) ? graphicClass.ToString() : null);
				array[2] = " at ";
				array[3] = req.path;
				array[4] = ": ";
				array[5] = ex.ToString();
				Log.Error(string.Concat(array));
			}
			return BaseContent.BadGraphic;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000FB114 File Offset: 0x000F9314
		private static T GetInner<T>(GraphicRequest req) where T : Graphic, new()
		{
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			req.renderQueue = ((req.renderQueue == 0 && req.graphicData != null) ? req.graphicData.renderQueue : req.renderQueue);
			Graphic graphic;
			if (!GraphicDatabase.allGraphics.TryGetValue(req, out graphic))
			{
				if (!UnityData.IsInMainThread)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Attempted to load a graphic off the main thread: ");
					defaultInterpolatedStringHandler.AppendFormatted<GraphicRequest>(req);
					Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), req.GetHashCode());
					return default(T);
				}
				graphic = Activator.CreateInstance<T>();
				graphic.Init(req);
				GraphicDatabase.allGraphics.Add(req, graphic);
			}
			return (T)((object)graphic);
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000FB1F4 File Offset: 0x000F93F4
		public static void Clear()
		{
			GraphicDatabase.allGraphics.Clear();
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000FB200 File Offset: 0x000F9400
		[DebugOutput("System", false)]
		public static void AllGraphicsLoaded()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("There are " + GraphicDatabase.allGraphics.Count.ToString() + " graphics loaded.");
			int num = 0;
			foreach (Graphic graphic in GraphicDatabase.allGraphics.Values)
			{
				stringBuilder.AppendLine(num.ToString() + " - " + graphic.ToString());
				if (num % 50 == 49)
				{
					Log.Message(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
				}
				num++;
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001CB2 RID: 7346
		private static Dictionary<GraphicRequest, Graphic> allGraphics = new Dictionary<GraphicRequest, Graphic>();

		// Token: 0x04001CB3 RID: 7347
		private static Dictionary<Type, Func<GraphicRequest, Graphic>> cachedGraphicGetters = new Dictionary<Type, Func<GraphicRequest, Graphic>>();
	}
}
