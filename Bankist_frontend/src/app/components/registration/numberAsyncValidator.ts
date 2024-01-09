import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable, of, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';

// Async validator za proveru da li je vrednost broj
export function numberAsyncValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
        return timer(400).pipe(
            switchMap(() => {
                const isNumber = /^\d+$/.test(control.value);
                const isValidLength = control.value.length >= 9 && control.value.length <= 10;

                return (isNumber && isValidLength) ? of(null) : of({ 'invalidNumber': true });
            })
        );
    };
}