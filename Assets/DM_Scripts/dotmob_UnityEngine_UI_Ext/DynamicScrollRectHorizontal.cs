/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

namespace Dotmob.UnityEngine.UI.Ext
{
	public class DynamicScrollRectHorizontal : DynamicScrollRect
	{
		protected override float GetDimension(Vector2 vector)
		{
			return vector.x;
		}

		protected override Vector2 GetVector(float value)
		{
			return new Vector2(value, 0f);
		}

		protected override Vector2 GetReverseVector(float value)
		{
			return new Vector2(0f, value);
		}

		protected override int OneOrMinusOne()
		{
			return 1;
		}

		protected override int ReverseOneOrMinusOne()
		{
			return -1;
		}

		protected override float GetViewSize()
		{
			return base.t.rect.width;
		}
	}
}
