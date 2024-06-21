namespace FinanceManager.Models.Caclulations;

public class MinHeap<T> where T : IComparable<T>
{
    private readonly List<T> _elements = [];

    public int Size => _elements.Count;

    public bool IsEmpty()
    {
        return Size == 0;
    }

    public T Min()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Heap is empty");
        return _elements[0];
    }

    public void Insert(T element)
    {
        _elements.Add(element);
        HeapifyUp(Size - 1);
    }

    public T ExtractMin()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Heap is empty");

        var min = _elements[0];
        _elements[0] = _elements[Size - 1];
        _elements.RemoveAt(Size - 1);
        HeapifyDown(0);

        return min;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;
            if (_elements[index].CompareTo(_elements[parentIndex]) >= 0) break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        while (index < Size / 2)
        {
            var leftChildIndex = 2 * index + 1;
            var rightChildIndex = 2 * index + 2;

            var smallerChildIndex = leftChildIndex;
            if (rightChildIndex < Size && _elements[rightChildIndex].CompareTo(_elements[leftChildIndex]) < 0)
            {
                smallerChildIndex = rightChildIndex;
            }

            if (_elements[index].CompareTo(_elements[smallerChildIndex]) <= 0) break;

            Swap(index, smallerChildIndex);
            index = smallerChildIndex;
        }
    }

    private void Swap(int index1, int index2)
    {
        var temp = _elements[index1];
        _elements[index1] = _elements[index2];
        _elements[index2] = temp;
    }
}
