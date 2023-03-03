import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorsInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toast: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(err => {
        if (err) {
          switch (err.status) {
            case 400:
              if (err.error.errors) {
                const modelStatusError = [];
                for (const key in err.error.errors) {
                  if (err.error.errors[key]) {
                    modelStatusError.push(err.error.errors[key]+" \n    ");
                  }
                }
                console.log("in IF in interceptors page");

                this.toast.error(modelStatusError.flat().toString());
               // throw modelStatusError.flat();

              }
              else if(typeof(err.error)=="object") {
                this.toast.error(err.statusText, err.status);
                console.log("in ELSE in interceptors page");

              }
              else{
                this.toast.error(err.error,err.status);
              }
              break;
            case 401:

            if(typeof(err.error)=="object") {
              this.toast.error(err.statusText, err.status);
              console.log("in ELSE in interceptors page");

            }
            else{
              this.toast.error(err.error,err.status);
            }
              // this.toast.error(err.statusText, err.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = { state: { error: err.error } };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toast.error("Something wrong happend",err.error);
              console.log(err.error);
              break;


          }
        }
        return throwError(err);
      })

    );
  }
}
