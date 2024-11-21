using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Core
{

    //Return Result to the controller level from Services
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }
        public static Result<T> Success(T Value) => new Result<T> { IsSuccess = true, Value = Value };
        public static Result<T> Failure(string Error) => new Result<T> { IsSuccess = false, Error = Error };

    }
    
        //if (user == null)
        //    return Result<LoginResponseDto>.Failure("Username invalid");

        //bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        //if (!isValid)
        //    return Result<LoginResponseDto>.Failure("Password invalid");



    //public static Result<T> Success(T value)
    //{
    //    Result<T> result = new Result<T>();
    //    result.IsSuccess = true;
    //    result.Value = value;
    //    return result;
    //}

    //public static Result<T> Failure(string error)
    //{
    //    Result<T> result = new Result<T>();
    //    result.IsSuccess = false;
    //    result.Error = error;
    //    return result;
    //}




}
