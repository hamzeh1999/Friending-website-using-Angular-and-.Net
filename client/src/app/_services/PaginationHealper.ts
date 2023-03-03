import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/Pagination";

export function getPaginationHeader(pageNumber: number, pageSize: number) {
    let parms = new HttpParams();
    parms = parms.append("pageNumber", pageNumber.toString());
    parms = parms.append("pageSize", pageSize.toString());

    return parms;

  }
  export function  getPaginatedResult<T>(url, params,http:HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();


    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null)
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));

        return paginatedResult;
      }
      ));

  }

