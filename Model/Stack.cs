using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Stack<T>
	{
		private Node<T> head;
		public Stack()
		{
			head = null;
		}
		public bool IsEmpty()
		{ return head == null; }
		public T Top()
		{ return head.GetValue(); }
		public void Push(T x)
		{ head = new Node<T>(x, head); }
		public T Pop()
		{
			T val = head.GetValue();
			Node<T> temp = head;
			head = head.GetNext();
			temp.SetNext(null);
			return val;
		}
		public override string ToString()
		{
			Node<T> temp = head;
			string s = "[";
			while (temp != null)
			{
				s += temp.GetValue().ToString();
				if (temp.GetNext() != null)
					s += ", ";
				temp = temp.GetNext();
			}
			s += "]";
			return s;
		}
	}
}
