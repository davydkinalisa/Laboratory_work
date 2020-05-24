using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_VP
{
    public class tckFRM
    {

        string _title, _hall, _row, _seat, _time, _number, _price;

        public tckFRM(string title, string hall, string row, string seat, string time, string number, string price)
        {
            _title = title;
            _hall = hall;
            _row = row;
            _seat = seat;
            _time = time;
            _number = number;
            _price = price;
        }

        public string title { get => _title; }
        public string hall { get => _hall; }
        public string row { get => _row; }
        public string seat { get => _seat; }
        public string time { get => _time; }
        public string number { get => _number; }
        public string price { get => _price; }

    }
}
