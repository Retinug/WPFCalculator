using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    delegate void CalculatorUpdateOutput(Calculator sender, double value, int precision);
    delegate void CalculatorInternalError(Calculator sender, string message);

    enum CalculatorUnOperations { Invert, Reciprocal, Backspace, Sqr, Sqrt }
    enum CalculatorOperations { Add, Sub, Mul, Div, Eq, Percent }


    class Calculator
    {
        double? input = null;
        double? result = null;

        int? fractionDigits = null;

        CalculatorOperations? op = null;

        public event CalculatorUpdateOutput didUpdateValue;
        public event CalculatorInternalError InputError;
        public event CalculatorInternalError ComputationError;
        public void AddDigit(int digit)
        {
            if (fractionDigits.HasValue == false)
            {
                if (input.HasValue && Math.Log10(input.Value) > 10)
                {
                    InputError?.Invoke(this, "Value is too long for this calculator");
                    return;
                }
                input = (input ?? 0) * 10 + digit;
            }
            else
            {
                fractionDigits++;
                input = (input ?? 0) + digit * Math.Pow(10, -fractionDigits.Value);

            }
            didUpdateValue?.Invoke(this, input.Value, fractionDigits ?? 0);
        }

        public void AddPoint()
        {
            if (!fractionDigits.HasValue)
                fractionDigits = 0;
        }

        public void ProcessUnOp(CalculatorUnOperations op)
        {
            input = input ?? 0;

            switch (op)
            {
                case CalculatorUnOperations.Invert:
                    input = -input;
                    break;

                case CalculatorUnOperations.Reciprocal:
                    if (input.Value == 0)
                        ComputationError?.Invoke(this, "Division by Zero");
                    else
                        input = 1 / input;
                    break;

                case CalculatorUnOperations.Sqr:
                    input = Math.Pow(input.Value, 2);
                    break;

                case CalculatorUnOperations.Sqrt:
                    input = Math.Sqrt(input.Value);
                    break;

                case CalculatorUnOperations.Backspace:
                    if (input < 10)
                        input = 0;
                    else
                    {
                        string line = input.Value.ToString().Remove(input.Value.ToString().Length - 1);
                        input = Convert.ToDouble(line);
                    }

                    break;
            }
            didUpdateValue?.Invoke(this, input.Value, fractionDigits ?? 0);

        }

        public void ProcessBinOp(CalculatorOperations op)
        {
            if (this.op.HasValue && input.HasValue && result.HasValue)
            {
                Compute();
                this.op = op;
            }
            else
                this.op = op;

            if (input.HasValue)
            {
                result = input;
                Clear();
            }
        }

        public void Compute()
        {
            switch (op)
            {
                case CalculatorOperations.Add:
                    result = result + input;
                    break;

                case CalculatorOperations.Sub:
                    result = result - input;
                    break;

                case CalculatorOperations.Mul:
                    result = result * input;
                    break;

                case CalculatorOperations.Div:
                    if (input.HasValue && input.Value == 0)
                    {
                        ComputationError?.Invoke(this, "Division by Zero");
                        return;
                    }
                    else
                    {
                        result = result / input;
                    }
                    break;

                case CalculatorOperations.Percent:
                    result = result * input / 100;
                    break;
            }

            didUpdateValue?.Invoke(this, result.Value, 0);
            input = null;
        }

        public void Clear()
        {
            input = null;
            fractionDigits = null;
            didUpdateValue?.Invoke(this, 0, 0);
        }

        public void ClearAll()
        {
            Clear();
            result = null;
        }
    }
    //delegate void CaclulatorUpdateOutput(Calculator sender, double value, int precision);
    //delegate void CaclulatorInernalError(Calculator sender, string message);
    //enum CalculatorOperation { Add, Sub, Mul, Div}
    //enum CalculatorTransformation { Negate, Percent, Sqr, Sqrt, Inverse }
    //class Calculator
    //{
    //    double? input = null;
    //    double? result = null;

    //    CalculatorOperation? op = null;

    //    bool hasPoint;
    //    int fractionDigits = 0;

    //    public event CaclulatorUpdateOutput DidUpdateValue;
    //    public event CaclulatorInernalError InputError;
    //    public event CaclulatorInernalError ComputationError;

    //    public void AddDigit(int digit)
    //    {
    //        if (hasPoint == false)
    //        {
    //            if (input.HasValue && Math.Log10(input.Value) > 9)
    //            {
    //                InputError?.Invoke(this, "Value is too long for this calculator!");
    //                return;
    //            }

    //            input = (input ?? 0) * 10 + digit;                
    //        }
    //        else
    //        {
    //            if (fractionDigits > 8)
    //            {
    //                InputError?.Invoke(this, "Value is too long for this calculator!");
    //                return;
    //            }
    //            fractionDigits += 1;
    //            input = (input ?? 0) + digit * Math.Pow(10, -fractionDigits);
    //        }
    //        DidUpdateValue?.Invoke(this, input.Value, fractionDigits);
    //    }

    //    public void AddPoint()
    //    {
    //        hasPoint = true;
    //    }

    //    public void TransformInput(CalculatorTransformation t)
    //    {
    //        input = input ?? 0;

    //        switch (t)
    //        {
    //            case CalculatorTransformation.Negate:
    //                input = -input;
    //                break;
    //        }
    //        DidUpdateValue?.Invoke(this, input.Value, fractionDigits);
    //    }

    //    public void AddOperation(CalculatorOperation op)
    //    {
    //        if (this.op.HasValue)
    //            Compute();

    //        this.op = op;
    //        if (input.HasValue)
    //        {
    //            result = input;
    //            this.Clear();
    //        }
    //    }

    //    public void Compute()
    //    {
    //        switch(this.op)
    //        {
    //            case CalculatorOperation.Add:
    //                result = result + input;
    //                DidUpdateValue?.Invoke(this, result.Value, 0);
    //                ResetInput();
    //                break;
    //            case CalculatorOperation.Div:
    //                if (input.HasValue && input.Value != 0)
    //                {
    //                    result = result / input;
    //                    DidUpdateValue?.Invoke(this, result.Value, 0);
    //                    ResetInput();
    //                }
    //                else
    //                    ComputationError?.Invoke(this, "Division by Zero");
    //                break;

    //        }
    //    }

    //    void ResetInput()
    //    {
    //        input = null;
    //        fractionDigits = 0;
    //        hasPoint = false;
    //    }

    //    public void Clear()
    //    {
    //        ResetInput();
    //        DidUpdateValue?.Invoke(this, 0, 0);
    //    }
    //}
}
