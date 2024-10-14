using System;

namespace MovementEffects
{
	public struct CoroutineHandle : IEquatable<CoroutineHandle>
	{
		private const byte ReservedSpace = 15;

		private static readonly int[] NextIndex;

		private readonly int _id;

		public byte Key => (byte)(_id & 0xF);

		public bool IsValid => Key != 0;

		public CoroutineHandle(byte ind)
		{
			if (ind > 15)
			{
				ind = (byte)(ind - 15);
			}
			_id = NextIndex[ind] + ind;
			NextIndex[ind] += 16;
		}

		public bool Equals(CoroutineHandle other)
		{
			return _id == other._id;
		}

		public override bool Equals(object other)
		{
			if (other is CoroutineHandle)
			{
				return Equals((CoroutineHandle)other);
			}
			return false;
		}

		public static bool operator ==(CoroutineHandle a, CoroutineHandle b)
		{
			return a._id == b._id;
		}

		public static bool operator !=(CoroutineHandle a, CoroutineHandle b)
		{
			return a._id != b._id;
		}

		public override int GetHashCode()
		{
			return _id;
		}

		static CoroutineHandle()
		{
			int[] array = new int[31];
			array[0] = 16;
			NextIndex = array;
		}
	}
}
