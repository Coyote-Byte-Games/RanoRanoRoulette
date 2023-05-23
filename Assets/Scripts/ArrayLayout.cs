using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArrayLayout<T>
{
	[System.Serializable]
	public struct Row
	{
		public T[] elements;
		public Row(int length)
		{
			elements = new T[length];
		}
	}

	public int rowCount, colCount;
	public Row[] rows; 
	public ArrayLayout(int columns,int rows)
	{
		this.rowCount = rows;
		this.colCount = columns;
		
		this.rows = new Row[rows];
		for (int i = 0; i < rows; i++)
		{
			this.rows[i] = new Row(columns);
		}
	}
	public T GetIndex(int x, int y)
	{
		//its fucking cancer cuz the first one is vertical columns
		return this.rows[y].elements[x];
	}
	public int GetColumnCount()
	{
		return colCount;
	}
	public int GetRowCount()
	{
		return rowCount;
	}

}
