using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonRespones
{
    public class Result
    {
        private readonly List<Error> _erros = [];
        public bool IsSuccess => _erros.Count == 0; // no errors => true

        public bool IsFailure => !IsSuccess; //have errors

        public IReadOnlyList<Error> errors => _erros;

        // success - ok 
        protected Result()
        {
            
        }
        //Failure with single one error
        protected Result(Error error)
        {
            _erros.Add(error);
        }

        // Failure witn many errors

        protected Result(List<Error> errors)
        {
            _erros.AddRange(errors);
        }

        //Factory Method
        public static Result Ok() => new Result();


        public static Result Fail(Error error) => new Result(error);

        public static Result Fail(List<Error> errors) => new Result(errors);
    }


    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public TValue Value => IsSuccess?_value:throw new InvalidOperationException("you can not access the value in case of Failure Scenario");
        private Result(TValue Value)
        {
            _value = Value;
        }

        private Result(Error error):base(error)
        {
            _value = default!;
        }

        private Result(List<Error> errors) : base(errors)
        {
            _value=default!;
        }

        public static Result<TValue> Ok(TValue value)=>new Result<TValue>(value);
        public new static Result<TValue> Fail(Error error)=>new(error); // syntax sugar he know what i return
        public new static Result<TValue> Fail(List<Error> errors)=>new Result<TValue>(errors);

        public static implicit operator Result<TValue>(TValue value)=>Ok(value);

        public static implicit operator Result<TValue>(Error error)=>Fail(error);

        public static implicit operator Result<TValue>(List<Error> errors)=>Fail(errors);


    }
}
