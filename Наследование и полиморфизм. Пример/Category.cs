using System;

namespace Inheritance.DataStructure
{
    public class Category : IComparable<Category>
    {
        public string Product { get; }
        public MessageType Type { get; }
        public MessageTopic Topic { get; }

        public Category(string product, MessageType type, MessageTopic topic)
        {
            Product = product;
            Type = type;
            Topic = topic;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Category other = (Category)obj;
            return Product == other.Product && Type == other.Type && Topic == other.Topic;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Product, Type, Topic).GetHashCode();
        }

        public int CompareTo(Category other)
        {
            if (other == null)
            {
                return 1;
            }

            int productComparison = string.Compare(Product, other.Product, StringComparison.Ordinal);
            if (productComparison != 0)
            {
                return productComparison;
            }

            int typeComparison = Type.CompareTo(other.Type);
            if (typeComparison != 0)
            {
                return typeComparison;
            }

            return Topic.CompareTo(other.Topic);
        }

        public static bool operator ==(Category left, Category right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Category left, Category right)
        {
            return !(left == right);
        }

        public static bool operator <(Category left, Category right)
        {
            if (left is null)
            {
                return right is object;
            }

            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Category left, Category right)
        {
            if (left is null)
            {
                return true;
            }

            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Category left, Category right)
        {
            return !(left <= right);
        }

        public static bool operator >=(Category left, Category right)
        {
            return !(left < right);
        }

        public override string ToString()
        {
            return $"{Product}.{Type}.{Topic}";
        }
    }
}