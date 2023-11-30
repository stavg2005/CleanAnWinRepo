using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Node<T>
	{
		private T value;
		private Node<T> next;

		public Node(T value)
		{
			this.value = value;
			this.next = null;
		}
		public Node(T value, Node<T> next)
		{
			this.value = value;
			this.next = next;
		}
		public T GetValue() { return value; }
		public void SetValue(T value) { this.value = value; }
		public Node<T> GetNext() { return next; }
		public void SetNext(Node<T> next) { this.next = next; }
		public override string ToString()
		{
			return this.value.ToString();
		}
	}
}
