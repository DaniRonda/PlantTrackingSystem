import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
    stages: [
        { duration: '1m', target: 100 },  
        { duration: '2m', target: 1000 }, 
        { duration: '1m', target: 100 },  
        { duration: '2m', target: 1000 },  
        { duration: '1m', target: 0 },
    ],
};

export default function () {
    http.get('http://localhost:5000/api/DataRecord/data');
    sleep(1);
}
