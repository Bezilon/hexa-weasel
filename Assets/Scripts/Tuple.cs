using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Tuple<T1, T2>
{
	public T1 Item1;
	public T2 Item2;
	
	private static readonly IEqualityComparer Item1Comparer = EqualityComparer<T1>.Default;
	private static readonly IEqualityComparer Item2Comparer = EqualityComparer<T2>.Default;
	
	public Tuple(T1 Item1, T2 Item2)
	{
		this.Item1 = Item1;
		this.Item2 = Item2;
	}
	
	public override string ToString()
	{
		return string.Format("<{0}, {1}>", Item1, Item2);
	}
	
	public static bool operator ==(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		if (Tuple<T1, T2>.IsNull(a) && !Tuple<T1, T2>.IsNull(b))
			return false;
		
		if (!Tuple<T1, T2>.IsNull(a) && Tuple<T1, T2>.IsNull(b))
			return false;
		
		if (Tuple<T1, T2>.IsNull(a) && Tuple<T1, T2>.IsNull(b))
			return true;
		
		return
			a.Item1.Equals(b.Item1) &&
				a.Item2.Equals(b.Item2);
	}
	
	public static bool operator !=(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		return !(a == b);
	}
	
	public override int GetHashCode()
	{
		int hash = 17;
		hash = hash * 23 + Item1.GetHashCode();
		hash = hash * 23 + Item2.GetHashCode();
		return hash;
	}
	
	public override bool Equals(object obj)
	{
		var other = obj as Tuple<T1, T2>;
		if (object.ReferenceEquals(other, null))
			return false;
		else
			return Item1Comparer.Equals(Item1, other.Item1) &&
				Item2Comparer.Equals(Item2, other.Item2);
	}
	
	private static bool IsNull(object obj)
	{
		return object.ReferenceEquals(obj, null);
	}
}